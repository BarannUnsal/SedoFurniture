using MensaApp.Domain.Entities;
using MensaApp.Domain.Entities.Common;
using MensaApp.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MensaApp.Persistence.Context
{
    public class MensaDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public MensaDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<About> Abouts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasMany(x => x.SubCategories)
               .WithOne(x => x.Category)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SubCategory>().HasMany(x => x.Products)
                .WithOne(x => x.SubCategory)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SubCategory>().HasOne(x => x.Category)
                .WithMany(x => x.SubCategories)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>().HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>().HasOne(x => x.SubCategory)
                .WithMany(x => x.Products)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>().HasMany(x => x.ProductImages)
                .WithOne(x => x.Product)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>().HasMany(x => x.ProjectImages)
                .WithOne(x => x.Project)
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var item in datas)
            {
                _ = item.State switch
                {
                    EntityState.Added => item.Entity.CreateDate = DateTime.UtcNow,
                    EntityState.Modified => item.Entity.UpdateDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow
                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}