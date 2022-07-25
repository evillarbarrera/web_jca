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
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;
        MailService MailService;
        IConfiguration configuration;
        public ContactController(ILogger<ContactController> logger, MailService MailService, IConfiguration configuration)
        {
            _logger = logger;

           this.MailService = MailService;
           this.configuration = configuration;
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EmailSendContact(EmailConfig _email)
        {
            sendMail(_email.To, _email.Subject,_email.Body);
            return View("Contact");
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

        public void sendMail(String nombre, String email, String mensaje)
        {
            if(nombre != null && email != null && mensaje!= null)
            {
                //string receptor, string asunto, string mensaje
                MailMessage mail = new MailMessage();

                String usermail = this.configuration["usuariogmail"];
                String passwordgmail = this.configuration["passwordgmail"];

                mail.From = new MailAddress(usermail);
                mail.To.Add(new MailAddress("contacto@jcasecurity.cl"));
                mail.Subject = nombre + " - " + email;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;

                string smtpserver = this.configuration["hostGmail"];
                int port = int.Parse(this.configuration["portGmail"]);
                bool ssl = bool.Parse(this.configuration["sslGmail"]);
                bool defaultcredentials = true;

                SmtpClient smtpClient = new SmtpClient();

                smtpClient.Host = smtpserver;
                smtpClient.Port = port;
                smtpClient.EnableSsl = ssl;
                smtpClient.UseDefaultCredentials = defaultcredentials;

                NetworkCredential usercredential = new NetworkCredential(usermail, passwordgmail);
                Console.WriteLine(usermail);
                Console.WriteLine(passwordgmail);
                Console.WriteLine(mail);
                smtpClient.Credentials = usercredential;

                try 
                {
                    smtpClient.Send(mail);
                    }
                catch(Exception e) 
                    {
                    Alert("No e ha enviado con exito", NotificationType.Error, "Contacto");
                }
                finally 
                    {
                    Alert("Se ha enviado con exito", NotificationType.Success, "Contacto");
                }
            }
            else
            {
                Alert("Faltan datos", NotificationType.Success, "Contacto");
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
