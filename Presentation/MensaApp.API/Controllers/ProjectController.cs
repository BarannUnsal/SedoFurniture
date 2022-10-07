using MensaApp.Application.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.Controllers
{
    [AllowAnonymous]
    public class ProjectController : Controller
    {
        readonly IProjectService _projectService;
        readonly ILogger<ProjectController> _logger;

        public ProjectController(IProjectService projectService, ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        [HttpGet]
        [Route("projects")]
        public IActionResult Projects()
        {
            try
            {
                _logger.LogInformation("Projeler sayfasına girildi");
            }
            catch (Exception ex)
            {

                _logger.LogInformation("Projeler sayfasına girilerken hata oluştu");
                return RedirectToAction("PageNotFound", "Error");
            }
            return View();
        }

        [HttpGet]
        [Route("project/{id}/{Name}")]
        public async Task<IActionResult> ProjectDetail(int id)
        {
            try
            {
                var project = await _projectService.GetByIdProjectAsync(id);
                if (project == null)
                {
                    _logger.LogError("Proje detay sayfasında 404 hatası");
                    return RedirectToAction("PageNotFound", "Error");
                }

                _logger.LogInformation("Proje detay sayfasına girildi");
                return View(project);
            }
            catch (Exception ex)
            {
                _logger.LogError("Proje detay sayfasında hata");
            }
            return View();
        }
    }
}
