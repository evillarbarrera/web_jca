using Microsoft.AspNetCore.Mvc;
using static JCA.Models.Enum;

namespace JCA.Controllers
{
    public class AlertController : BaseController
    {
        [HttpGet]
        public ActionResult ShowAlert()
        {
            Alert("", NotificationType.Success,"Envio de CV");
            return View();
        }
    }
}