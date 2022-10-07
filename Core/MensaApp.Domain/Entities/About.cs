using MensaApp.Domain.Entities.Common;

namespace MensaApp.Domain.Entities
{
    public class About : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
