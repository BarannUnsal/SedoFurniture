using System.ComponentModel.DataAnnotations;

namespace MensaApp.Domain.Entities.Common
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        virtual public DateTime UpdateDate { get; set; }
    }
}
