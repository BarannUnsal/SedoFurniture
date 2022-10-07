using MensaApp.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.ViewComponents.SiteMap
{
    public class SiteMapCategories : ViewComponent
    {
        readonly ICategoryService categoryService;

        public SiteMapCategories(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var category = categoryService.GetCategoryWithSubCategory().ToList();
            return View(category);
        }
    }
}
