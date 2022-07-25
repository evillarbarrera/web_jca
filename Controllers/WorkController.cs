using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JCA.Models;
using jca.Helpers;
using Microsoft.AspNetCore.Http;
using System.IO;
using static JCA.Models.Enum;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;

namespace JCA.Controllers
{
    public class WorkController : Controller
    {
        IConfiguration configuration;
        private readonly ILogger<WorkController> _logger;
        MailService MailService;
        public WorkController(ILogger<WorkController> logger, MailService MailService, IConfiguration configuration)
        {
            _logger = logger;
            this.MailService = MailService;
             this.configuration = configuration;
        }
        

        public IActionResult Work()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EmailSend(EmailConfig _email)
        {
            
            sendMailAdjunto(_email.To, _email.Subject,_email.Body,_email.Attachment);
            return View("Work");

        }

         public void Alert(string message, NotificationType type, string title)
        {
             var msg = new
            {
                message = message,
                title = title,
                type = type,
                provider = GetProvider()
            };
           
            TempData["notification"] = JsonConvert.SerializeObject(msg);
        }

         public void sendMailAdjunto(String nombre, String email, String mensaje, IFormFile attachments)
        {
            if(nombre != null && email != null && mensaje!= null)
            {
 
            MailMessage mail = new MailMessage();

            String usermail = this.configuration["usuariogmail"];
            String passwordmail = this.configuration["passwordgmail"];
            mail.From = new MailAddress(usermail);
            mail.To.Add(new MailAddress("contacto@jcasecurity.cl"));
            mail.Subject = nombre + " - " + email;
            mail.Body = mensaje;

            if (attachments != null)
                {
                    string fileName = Path.GetFileName(attachments.FileName);
                    mail.Attachments.Add(new Attachment(attachments.OpenReadStream(), fileName));
                }

            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;

            string smtpserver = this.configuration["hostGmail"];
            int port = int.Parse(this.configuration["portGmail"]);
            bool ssl = bool.Parse(this.configuration["sslGmail"]);
            bool defaultcredentials = bool.Parse(this.configuration["defaultcredencialsGmail"]);

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Host = smtpserver;
            smtpClient.Port = port;
            smtpClient.EnableSsl = ssl;
            smtpClient.UseDefaultCredentials = defaultcredentials;

            NetworkCredential usercredential = new NetworkCredential(usermail, passwordmail);

            smtpClient.Credentials = usercredential;


            try 
            {
                 smtpClient.Send(mail);
                }
            catch(Exception e) 
                {
                Alert("Se ha enviado con exito", NotificationType.Error, "Contactanos");

            }
            finally 
                {
              
                Alert("Se ha enviado con exito", NotificationType.Success, "Contactanos");
            }
           
            }
            else
            {
                 Alert("Faltan datos para llenar", NotificationType.Info, "Contactanos");
            }
          

    
        }

         private string GetProvider()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var value = configuration["NotificationProvider"];

            return value;
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
