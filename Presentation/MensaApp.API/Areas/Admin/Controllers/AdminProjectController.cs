using FluentValidation.Results;
using MensaApp.API.Areas.Admin.Models.Projects;
using MensaApp.Application.Abstraction.Services;
using MensaApp.Application.Pagination;
using MensaApp.Domain.Entities;
using MensaApp.Infrastructure.Validations.ProjectValidate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace MensaApp.API.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminProjectController : Controller
    {
        readonly IProjectService _projectService;
        readonly IToastNotification _toastNotification;
        readonly IWebHostEnvironment _webHostEnvironment;

        public AdminProjectController(IProjectService projectService, IWebHostEnvironment webHostEnvironment, IToastNotification toastNotification)
        {
            _projectService = projectService;
            _webHostEnvironment = webHostEnvironment;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        public IActionResult Projects(string? sortOrder, string? currentFilter, string? searchString, int? pageNumber)
        {
            try
            {
                ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
                ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
                ViewData["CreateDateSortParm"] = sortOrder == "CreateDate" ? "createDate_desc" : "CreateDate";
                ViewData["UpdateDateSortParm"] = sortOrder == "UpdateDate" ? "updateDate_desc" : "UpdateDate";
                ViewData["CurrentFilter"] = searchString;
                ViewData["CurrentSort"] = sortOrder;

                ViewBag.projectCount = _projectService.GetAll().Count().ToString();

                var projects = from x in _projectService.GetAll().ToList()
                               select x;


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
                    projects = projects.Where(x => x.Name.ToUpper().Contains(searchString.ToUpper()));
                }

                switch (sortOrder)
                {
                    case "id_desc":
                        projects = projects.OrderByDescending(x => x.Id);
                        break;
                    case "Name":
                        projects = projects.OrderBy(x => x.Name);
                        break;
                    case "name_desc":
                        projects = projects.OrderByDescending(x => x.Name);
                        break;
                    case "CreateDate":
                        projects = projects.OrderBy(x => x.CreateDate);
                        break;
                    case "createDate_desc":
                        projects = projects.OrderByDescending(x => x.CreateDate);
                        break;
                    case "UpdateDate":
                        projects = projects.OrderBy(x => x.UpdateDate);
                        break;
                    case "updateDate_desc":
                        projects = projects.OrderByDescending(x => x.UpdateDate);
                        break;
                    default:
                        projects = projects.OrderBy(x => x.Id);
                        break;
                }
                int pageSize = 5;
                return View(PaginatedList<Project>.Create(projects.ToList(), pageNumber ?? 1, pageSize));
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProjectDetail(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            return View(project);
        }

        [HttpGet]
        public IActionResult CreateProject()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectViewModel model)
        {
            Project project = new();
            project.Name = model.Name;
            project.Description = model.Description;
            project.CreateDate = DateTime.UtcNow;
            project.ProjectImages = model.ProjectImages;
            if (project.ImageUrl == null)
            {
                var extension = Path.GetExtension(model.ImageUrl.FileName);
                var newImageName = Guid.NewGuid().ToString() + "__" + project.Name + extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/projeler/", newImageName);
                var stream = new FileStream(location, FileMode.Create);
                model.ImageUrl.CopyTo(stream);
                project.ImageUrl = newImageName;
            }
            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                foreach (var item in files)
                {
                    var location = Path.Combine(webRootPath, "ProjeResimleri");
                    var extension = Path.GetExtension(item.FileName);
                    var newImageName = Guid.NewGuid().ToString() + "__" + project.Name + extension;

                    using (var filesStream = new FileStream(Path.Combine(location, newImageName), FileMode.Create))
                    {
                        item.CopyTo(filesStream);
                    }
                    model.ProjectImages.Add(new ProjectImage { ImageName = newImageName });
                }
            }
            ProjectValidation validations = new();
            ValidationResult result = validations.Validate(project);
            if (result.IsValid)
            {
                _toastNotification.AddSuccessToastMessage($"{model.Name} başarıyla eklendi", new NotyOptions()
                {
                    Theme = "metroui"
                });
                await _projectService.AddAsync(project);
                return RedirectToAction("Projects");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.ErrorMessage, item.PropertyName);
                }
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProject(int id)
        {
            var projectValue = await _projectService.GetByIdAsync(id);
            return View(projectValue);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditProject(UpdateProjectViewModel model)
        {
            var project = await _projectService.GetByIdAsync(model.Id);
            project.Name = model.Name;
            project.Description = model.Description;
            project.UpdateDate = DateTime.UtcNow;
            project.ProjectImages = model.ProjectImages;
            if (project.ImageUrl != null)
            {
                if (model.ImageUrl != null)
                {
                    var extension = Path.GetExtension(model.ImageUrl.FileName);
                    var newImageName = Guid.NewGuid() + extension;
                    var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/projeler/", newImageName);
                    var stream = new FileStream(location, FileMode.Create);
                    model.ImageUrl.CopyTo(stream);
                    project.ImageUrl = newImageName;
                }
            }
            string webRootPath = _webHostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                foreach (var item in files)
                {
                    var location = Path.Combine(webRootPath, "ProjeResimleri");
                    var extension = Path.GetExtension(item.FileName);
                    var newImageName = Guid.NewGuid().ToString() + "__" + project.Name + extension;

                    using (var filesStream = new FileStream(Path.Combine(location, newImageName), FileMode.Create))
                    {
                        item.CopyTo(filesStream);
                    }
                    model.ProjectImages.Add(new ProjectImage { ImageName = newImageName });
                }
            }
            ProjectValidation validations = new();
            ValidationResult result = validations.Validate(project);
            if (result.IsValid)
            {
                _toastNotification.AddSuccessToastMessage($"{model.Name} başarıyla eklendi", new NotyOptions()
                {
                    Theme = "metroui"
                });
                _projectService.Update(project);
                return RedirectToAction("Projects");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.ErrorMessage, item.PropertyName);
                }
                return View();
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProject(int id)
        {
            await _projectService.RemoveAsync(id);
            _toastNotification.AddErrorToastMessage("Proje başarıyla silindi");
            return RedirectToAction("Projects");
        }
    }
}
