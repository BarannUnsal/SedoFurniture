using MensaApp.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.Areas.Admin.ViewComponents.Dashboard
{
    public class Last10Product : ViewComponent
    {
        readonly IProductService _productService;

        public Last10Product(IProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke()
        {
            var product = _productService.GetLast10Product();
            return View(product);
        }
    }
}
