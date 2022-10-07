using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.Controllers
{
    [AllowAnonymous]
    public class SiteMapController : Controller
    {
        [Route("/site-map")]
        public IActionResult SiteMap()
        {
            return View();
        }
    }
}
