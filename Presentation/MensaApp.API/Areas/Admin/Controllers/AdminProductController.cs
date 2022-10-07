using FluentValidation.Results;
using MensaApp.API.Areas.Admin.Models.Products;
using MensaApp.Application.Abstraction.Services;
using MensaApp.Application.Pagination;
using MensaApp.Domain.Entities;
using MensaApp.Infrastructure.Validations.ProductValidate.CreateProduct;
using MensaApp.Infrastructure.Validations.ProductValidate.UpdateProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;

namespace MensaApp.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminProductController : Controller
    {
        readonly IProductService _productService;
        readonly ICategoryService _categoryService;
        readonly ISubCategoryService _subCategoryService;
        readonly IWebHostEnvironment _webHostEnvironment;
        readonly IToastNotification _toastNotification;
        readonly ILogger<AdminProductController> logger;
        public AdminProductController(IProductService productService, ICategoryService categoryService, ISubCategoryService subCategoryService, IWebHostEnvironment webHostEnvironment, IToastNotification toastNotification, ILogger<AdminProductController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
            _webHostEnvironment = webHostEnvironment;
            _toastNotification = toastNotification;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult GetProducts(string? sortOrder, string? currentFilter, string? searchString, int? pageNumber)
        {
            try
            {
                ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
                ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
                ViewData["CategorySortParm"] = sortOrder == "Category" ? "cat_desc" : "Category";
                ViewData["SubCategorySortParm"] = sortOrder == "SubCat" ? "subCat_desc" : "SubCat";
                ViewData["CreateDateSortParm"] = sortOrder == "CreateDate" ? "createDate_desc" : "CreateDate";
                ViewData["UpdateDateSortParm"] = sortOrder == "UpdateDate" ? "updateDate_desc" : "UpdateDate";
                ViewData["CurrentFilter"] = searchString;
                ViewData["CurrentSort"] = sortOrder;

                var products = from x in _productService.GetProductWithCategory()
                               select x;

                ViewBag.productCount = _productService.GetProductWithCategory().Count().ToString();

                if (searchString != null)
                {
                    pageNumber = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    products = products.Where(x => x.Name.ToUpper().Contains(searchString.ToUpper())
                                          || x.Category.CategoryName.ToUpper().Contains(searchString.ToUpper())
                                          || x.SubCategory.Name.ToUpper().Contains(searchString.ToUpper()));
                }

                switch (sortOrder)
                {
                    case "id_desc":
                        products = products.OrderByDescending(x => x.Id);
                        break;
                    case "Name":
                        products = products.OrderBy(x => x.Name);
                        break;
                    case "name_desc":
                        products = products.OrderByDescending(x => x.Name);
                        break;
                    case "Category":
                        products = products.OrderBy(x => x.Category.CategoryName);
                        break;
                    case "cat_desc":
                        products = products.OrderByDescending(x => x.Category.CategoryName);
                        break;
                    case "SubCat":
                        products = products.OrderBy(x => x.SubCategory.Name);
                        break;
                    case "subCat_desc":
                        products = products.OrderByDescending(x => x.SubCategory.Name);
                        break;
                    case "CreateDate":
                        products = products.OrderBy(x => x.CreateDate);
                        break;
                    case "createDate_desc":
                        products = products.OrderByDescending(x => x.CreateDate);
                        break;
                    case "UpdateDate":
                        products = products.OrderBy(x => x.UpdateDate);
                        break;
                    case "updateDate_desc":
                        products = products.OrderByDescending(x => x.UpdateDate);
                        break;
                    default:
                        products = products.OrderBy(x => x.Id);
                        break;
                }
                int pageSize = 5;
                return View(PaginatedList<Product>.Create(products.ToList(), pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                logger.LogError("Ürünler sayfasında hata ile karşılaşıldı ->>", ex);
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            List<SelectListItem> categoryValue = (from x in _categoryService.GetAll()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CategoryName,
                                                      Value = x.Id.ToString()
                                                  }).ToList();
            ViewBag.ListOfCategory = categoryValue;
            return View();
        }

        public IActionResult GetSubCategory(int cid)
        {
            var SubCategory_List = _subCategoryService.GetAll().Where(s => s.CategoryId == cid).Select(c => new { Id = c.Id, Name = c.Name }).ToList();
            return Json(SubCategory_List);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductViewModel model)
        {
            try
            {
                Product product = new();
                product.Name = model.Name;
                product.Description = model.Description;
                product.IsActive = model.IsActive;
                product.CategoryId = model.CategoryId;
                product.SubCategoryId = model.SubCategoryId;
                product.UpdateDate = DateTime.UtcNow;
                product.ProductImages = model.ProductImages;
                if (model.Thumbnail != null)
                {
                    var extension = Path.GetExtension(model.Thumbnail.FileName);
                    var newImageName = Guid.NewGuid() + extension;
                    var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/thumbnail/", newImageName);
                    var stream = new FileStream(location, FileMode.Create);
                    model.Thumbnail.CopyTo(stream);
                    product.Thumbnail = newImageName;
                }
                string webRootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    foreach (var item in files)
                    {
                        var location = Path.Combine(webRootPath, "ProductImages");
                        var extension = Path.GetExtension(item.FileName);
                        var newImageName = Guid.NewGuid().ToString() + "__" + product.Name + extension;

                        using (var filesStream = new FileStream(Path.Combine(location, newImageName), FileMode.Create))
                        {
                            item.CopyTo(filesStream);
                        }
                        model.ProductImages.Add(new ProductImage { ImageName = newImageName });
                    }
                }
                CreateProductValidation validations = new();
                ValidationResult result = validations.Validate(product);
                if (result.IsValid)
                {
                    _toastNotification.AddSuccessToastMessage($"{model.Name} başarıyla eklendi", new NotyOptions()
                    {
                        Theme = "metroui"
                    });
                    await _productService.AddAsync(product);
                    return RedirectToAction("GetProducts");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            try
            {
                var productValue = await _productService.GetByIdAsync(id);
                List<SelectListItem> categoryValue = (from x in _categoryService.GetAll()
                                                      select new SelectListItem
                                                      {
                                                          Text = x.CategoryName,
                                                          Value = x.Id.ToString()
                                                      }).ToList();
                ViewBag.ListOfCategory = categoryValue;
                return View(productValue);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductUpdateViewModel model)
        {
            try
            {
                var product = await _productService.GetByIdAsync(model.Id);
                product.UpdateDate = DateTime.UtcNow;
                product.Name = model.Name;
                product.Description = model.Description;
                product.SubCategoryId = model.SubCategoryId;
                product.CategoryId = model.CategoryId;
                product.IsActive = model.IsActive;
                product.ProductImages = model.ProductImages;
                if (model.Thumbnail != null)
                {
                    var extension = Path.GetExtension(model.Thumbnail.FileName);
                    var newImageName = Guid.NewGuid() + extension;
                    var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/thumbnail/", newImageName);
                    var stream = new FileStream(location, FileMode.Create);
                    model.Thumbnail.CopyTo(stream);
                    product.Thumbnail = newImageName;
                }
                string webRootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    foreach (var item in files)
                    {
                        var location = Path.Combine(webRootPath, "ProductImages");
                        var extension = Path.GetExtension(item.FileName);
                        var newImageName = Guid.NewGuid().ToString() + "__" + product.Name + extension;

                        using (var filesStream = new FileStream(Path.Combine(location, newImageName), FileMode.Create))
                        {
                            item.CopyTo(filesStream);
                        }
                        model.ProductImages.Add(new ProductImage { ImageName = newImageName });
                    }
                }
                _productService.Update(product);
                UpdateProductValidation validations = new();
                ValidationResult result = validations.Validate(product);
                if (result.IsValid)
                {
                    _toastNotification.AddSuccessToastMessage($"{model.Name} başarıyla güncellendi", new NotyOptions()
                    {
                        Theme = "metroui"
                    });
                    return RedirectToAction("GetProducts");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                    return View(product);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            try
            {
                var product = await _productService.RemoveAsync(id);
                _toastNotification.AddErrorToastMessage("Ürün başarıyla silindi");
                return RedirectToAction("GetProducts");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
