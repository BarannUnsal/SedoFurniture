using MensaApp.Application.Repository.AboutRepo;
using MensaApp.Domain.Entities;
using MensaApp.Persistence.Context;

namespace MensaApp.Persistence.Repository.AboutRepo
{
    public class AboutReadRepository : ReadRepository<About>, IAboutReadRepository
    {
        public AboutReadRepository(MensaDbContext context) : base(context)
        {
        }
    }
}
