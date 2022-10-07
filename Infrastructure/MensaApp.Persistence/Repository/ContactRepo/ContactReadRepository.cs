using MensaApp.Application.Repository.ContactRepo;
using MensaApp.Domain.Entities;
using MensaApp.Persistence.Context;

namespace MensaApp.Persistence.Repository.ContactRepo
{
    public class ContactReadRepository : ReadRepository<Contact>, IContactReadRepository
    {
        public ContactReadRepository(MensaDbContext context) : base(context)
        {
        }
    }
}
