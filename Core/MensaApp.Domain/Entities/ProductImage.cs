using MensaApp.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MensaApp.Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public string ImageName { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
