using MensaApp.Application.Repository.ProjectRepo;
using MensaApp.Domain.Entities;
using MensaApp.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MensaApp.Persistence.Repository.ProjectRepo
{
    public class ProjectReadRepository : ReadRepository<Project>, IProjectReadRepository
    {
        private readonly MensaDbContext context;

        public ProjectReadRepository(MensaDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Project> GetProjectIdAsync(int id)
        {
            return await context.Projects.Where(x => x.Id == id).Include(x => x.ProjectImages).FirstOrDefaultAsync();
        }
    }
}
