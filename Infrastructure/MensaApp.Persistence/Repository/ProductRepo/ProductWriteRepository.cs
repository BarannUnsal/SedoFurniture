using MensaApp.Application.Repository.ProductRepo;
using MensaApp.Domain.Entities;
using MensaApp.Persistence.Context;

namespace MensaApp.Persistence.Repository.ProductRepo
{
    public class ProductWriteRepository : WriteRepository<Product>, IProductWriteRepository
    {
        public ProductWriteRepository(MensaDbContext context) : base(context)
        {
        }
    }
}
