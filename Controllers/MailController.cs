using Microsoft.AspNetCore.Mvc;
using jca.Helpers;
using System;
using System.Collections.Generic;

namespace jca.Controllers
{
    public class MailController : Controller
    {
        MailService MailService;
        public MailController(MailService MailService)
        {
            this.MailService = MailService;
        }

        public IActionResult index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(String nombre, String email, String mensaje)
        {

            this.MailService.sendMail(email,nombre,mensaje);
            ViewData["MENSAJE"] = "email enviado a ";
            return View();
        }
        
    }
}