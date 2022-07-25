using Microsoft.AspNetCore.Mvc;
using static JCA.Models.Enum;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace JCA.Controllers
{
    public class BaseController : Controller
    {
        public void Alert(string message, NotificationType type, string title)
        {
            //TempData["notification"] = $"Swal.fire('{title}','{message}','{type.ToString().ToLower()}'";
            //TempData["notification"] = $"Swal.fire('Good job!','You clicked the button!','success')";
            var msg = new
            {
                message = message,
                title = title,
                type = type,
                provider = GetProvider()
            };
           
            TempData["Message"] = JsonConvert.SerializeObject(msg);
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

    }
}