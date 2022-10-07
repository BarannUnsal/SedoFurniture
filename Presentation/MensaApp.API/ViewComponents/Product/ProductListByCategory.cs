using MensaApp.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.ViewComponents.Product
{
    public class ProductListByCategory : ViewComponent
    {
        readonly IProductService _productService;

        public ProductListByCategory(IProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke(int id)
        {
            var product = _productService.GetProductWithCategoryId(x => x.CategoryId == id).ToList();
            if (product == null)
            {
                return View("../../Views/Error/PageNotFound.cshtml");
            }
            return View(product);
        }
    }
}
