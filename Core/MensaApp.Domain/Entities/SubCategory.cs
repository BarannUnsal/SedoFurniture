using MensaApp.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MensaApp.Domain.Entities
{
    public class SubCategory : BaseEntity
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Product>? Products { get; set; }
        [NotMapped]
        public string EncrypedId { get; set; }
    }
}
