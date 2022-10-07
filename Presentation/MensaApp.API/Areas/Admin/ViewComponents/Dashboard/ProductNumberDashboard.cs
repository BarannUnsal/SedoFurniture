using MensaApp.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace MensaApp.API.Areas.Admin.ViewComponents.Dashboard
{
    public class ProductNumberDashboard : ViewComponent
    {
        readonly IProductService _productService;
        readonly ICategoryService _categoryService;
        readonly ISubCategoryService _subCategoryService;
        readonly IProjectService _projectService;

        public ProductNumberDashboard(IProductService productService, ICategoryService categoryService, ISubCategoryService subCategoryService, IProjectService projectService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
            _projectService = projectService;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.productCount = _productService.GetAll().Count();
            ViewBag.categoryCount = _categoryService.GetAll().Count();
            ViewBag.subCategoryCount = _subCategoryService.GetAll().Count();
            ViewBag.projectCount = _projectService.GetAll().Count();

            string connection = "https://api.openweathermap.org/data/2.5/weather?q=istanbul&mode=xml&lang=tr&units=metric&appid=c08c0bc86ee03598f228447e28defbb9";
            XDocument document = XDocument.Load(connection);
            var wth = document.Descendants("temperature").ElementAt(0).Attribute("value").Value;
            ViewBag.weather = wth;

            return View();
        }
    }
}
