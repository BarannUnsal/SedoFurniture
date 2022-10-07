using MensaApp.Application.Repository.AboutRepo;
using MensaApp.Application.Repository.CategoryRepo;
using MensaApp.Application.Repository.ContactRepo;
using MensaApp.Application.Repository.ProductRepo;
using MensaApp.Application.Repository.ProjectRepo;
using MensaApp.Application.Repository.SubCategoryRepo;
using MensaApp.Domain.Entities.Identity;
using MensaApp.Persistence.Context;
using MensaApp.Persistence.CustomValidation.AdminValidate.UpdateAdmin;
using MensaApp.Persistence.Repository.AboutRepo;
using MensaApp.Persistence.Repository.CategoryRepo;
using MensaApp.Persistence.Repository.ContactRepo;
using MensaApp.Persistence.Repository.ProductRepo;
using MensaApp.Persistence.Repository.ProjectRepo;
using MensaApp.Persistence.Repository.SubCategoryRepo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MensaApp.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                "abcçdefgğhijklmnopqrstuüvwxyzABCÇDEFGĞHIİJKLMNOÖPQRSTÜUVWXYZ";
                options.Password.RequiredLength = 3;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddErrorDescriber<UpdateAdminValidation>().AddEntityFrameworkStores<MensaDbContext>().AddDefaultTokenProviders();


            services.AddScoped<IAboutReadRepository, AboutReadRepository>();
            services.AddScoped<IAboutWriteRepository, AboutWriterRepository>();
            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();
            services.AddScoped<IContactReadRepository, ContactReadRepository>();
            services.AddScoped<IContactWriteRepository, ContactWriteRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddScoped<ISubCategoryReadRepository, SubCategoryReadRepository>();
            services.AddScoped<ISubCategoryWriteRepository, SubCategoryWriteRepository>();
            services.AddScoped<IProjectReadRepository, ProjectReadRepository>();
            services.AddScoped<IProjectWriteRepository, ProjectWriteRepository>();
        }
    }
}
