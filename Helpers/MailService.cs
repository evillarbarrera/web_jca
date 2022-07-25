using System;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Http;
using JCA.Controllers;
using Microsoft.AspNetCore.Mvc;
using static JCA.Models.Enum;

namespace jca.Helpers
{
  
    public class MailService : BaseController
    {
        IConfiguration configuration;

        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
            
        }

      


        // 
        public void sendMail(String nombre, String email, String mensaje)
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
            smtpClient.Send(mail);
        }

        public void sendMailAdjunto(String nombre, String email, String mensaje, IFormFile attachments)
        {


            //string receptor, string asunto, string mensaje
            MailMessage mail = new MailMessage();
            // String x = fileUploader.FileName;

            String usermail = this.configuration["usuariogmail"];
            String passwordmail = this.configuration["passwordgmail"];
            mail.From = new MailAddress(usermail);
            mail.To.Add(new MailAddress("e.villarbarrera@gmail.com"));
            mail.Subject = nombre + " - " + email;
            mail.Body = mensaje;

            if (attachments.Length > 0)
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
           

             BaseController alert = new BaseController();
            try 
            {
                 smtpClient.Send(mail);
                }
            catch(Exception e) 
                {
                // 
                //alert.BasicNotification("Se ha enviado con exito", NotificationType.Success, "Envio de CV");
                
                alert.Alert("Se ha enviado con exito", NotificationType.Success, "Envio de CV");


            }
            finally 
                {
                //alert.BasicNotification("Se ha enviado su CV", NotificationType.Error, "Envio de CV");
                alert.Alert("Se ha enviado con exito", NotificationType.Success, "Envio de CV");
            }
           

    
        }

        

    }
}
