using MensaApp.Domain.Entities;

namespace MensaApp.API.Areas.Admin.Models.Products
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public IFormFile Thumbnail { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
