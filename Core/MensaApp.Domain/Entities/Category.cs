using MensaApp.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MensaApp.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public List<Product>? Products { get; set; }
        public List<SubCategory>? SubCategories { get; set; }

        [NotMapped]
        public string EncrypedId { get; set; }
    }
}