using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using System.Net.Mail;
 public class EmailConfig
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IFormFile Attachment { get; set; }
    }