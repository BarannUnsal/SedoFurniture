using MensaApp.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MensaApp.Domain.Entities
{
    public class Contact : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public bool MessageStatus { get; set; }
        [NotMapped]
        public string EncrypedId { get; set; }
    }
}
