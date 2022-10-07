using MensaApp.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MensaApp.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MensaDbContext>
    {
        public MensaDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<MensaDbContext> optionsBuilder = new();
            optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            return new(optionsBuilder.Options);
        }
    }
}
