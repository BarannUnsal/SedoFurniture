using MensaApp.Application.Repository.ContactRepo;
using MensaApp.Domain.Entities;
using MensaApp.Persistence.Context;

namespace MensaApp.Persistence.Repository.ContactRepo
{
    public class ContactWriteRepository : WriteRepository<Contact>, IContactWriteRepository
    {
        public ContactWriteRepository(MensaDbContext context) : base(context)
        {
        }
    }
}
