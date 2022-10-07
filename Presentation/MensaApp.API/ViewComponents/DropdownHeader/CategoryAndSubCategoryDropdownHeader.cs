using MensaApp.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.ViewComponents.DropdownHeader
{
    public class CategoryAndSubCategoryDropdownHeader : ViewComponent
    {
        readonly ICategoryService _categoryService;

        public CategoryAndSubCategoryDropdownHeader(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var category = _categoryService.GetCategoryWithSubCategory();
            return View(category);
        }
    }
}
