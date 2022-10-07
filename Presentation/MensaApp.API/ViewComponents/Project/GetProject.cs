using MensaApp.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace MensaApp.API.ViewComponents.Project
{
    public class GetProject : ViewComponent
    {
        readonly IProjectService _projectService;

        public GetProject(IProjectService projectService)
        {
            _projectService = projectService;
        }
        public IViewComponentResult Invoke()
        {
            var project = _projectService.GetAll().ToList();
            return View(project);
        }
    }
}
