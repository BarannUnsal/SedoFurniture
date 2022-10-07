using Mapster;
using MensaApp.API.Areas.Admin.Models.Admin;
using MensaApp.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.ViewComponents.AdminNameNavbar
{
    public class NavbarOnAdminNav : ViewComponent
    {
        readonly UserManager<AppUser> userManager;

        public NavbarOnAdminNav(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = userManager.FindByNameAsync(User.Identity.Name).Result;

                AdminViewModel adminView = user.Adapt<AdminViewModel>();
                return View(adminView);
            }
            return View();
        }
    }
}
