using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminDashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
