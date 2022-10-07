using MensaApp.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MensaApp.Domain.Entities
{
    public partial class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description{ get; set; }
        public string Thumbnail { get; set; }
        public bool IsActive { get; set; }
        public int? CategoryId { get; set; } 
        public virtual Category Category { get; set; }
        public int? SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public List<ProductImage> ProductImages { get; set; }
    }
    public partial class Product
    {
        [NotMapped]
        public string EncrypedId { get; set; }
    }
}