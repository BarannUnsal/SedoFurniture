using MensaApp.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.ViewComponents.Product
{
    public class ProductWithSubCategory : ViewComponent
    {
        readonly IProductService _productService;

        public ProductWithSubCategory(IProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke(int id)
        {
            var product = _productService.GetProductWithSubCategoryId(x => x.SubCategoryId == id).ToList();
            return View(product);
        }
    }
}
