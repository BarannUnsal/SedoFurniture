using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult PageNotFound(int code)
        {
            return View();
        }
    }
}
