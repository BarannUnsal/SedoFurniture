using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.ViewComponents.Product
{
    public class ProductOnHomePage : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
