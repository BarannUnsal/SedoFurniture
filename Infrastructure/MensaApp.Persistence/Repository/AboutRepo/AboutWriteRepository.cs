using MensaApp.Application.Repository.AboutRepo;
using MensaApp.Domain.Entities;
using MensaApp.Persistence.Context;

namespace MensaApp.Persistence.Repository.AboutRepo
{
    public class AboutWriterRepository : WriteRepository<About>, IAboutWriteRepository
    {
        public AboutWriterRepository(MensaDbContext context) : base(context)
        {
        }
    }
}
