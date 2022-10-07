using MensaApp.Domain.Entities;

namespace MensaApp.API.Areas.Admin.Models.Projects
{
    public class UpdateProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile ImageUrl { get; set; }
        public List<ProjectImage> ProjectImages { get; set; }
    }
}
