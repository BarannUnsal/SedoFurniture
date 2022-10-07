using MensaApp.Domain.Entities.Common;

namespace MensaApp.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public List<ProjectImage> ProjectImages { get; set; }
    }
}
