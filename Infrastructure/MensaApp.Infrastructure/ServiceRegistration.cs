using FluentValidation.AspNetCore;
using MensaApp.Application.Abstraction.Services;
using MensaApp.Infrastructure.Filters;
using MensaApp.Infrastructure.Services.Managers;
using MensaApp.Infrastructure.Validations.AboutValidate.CreateAbout;
using MensaApp.Infrastructure.Validations.AboutValidate.UpdateAbout;
using MensaApp.Infrastructure.Validations.CategoryValidate.CreateCategory;
using MensaApp.Infrastructure.Validations.CategoryValidate.UpdateCategory;
using MensaApp.Infrastructure.Validations.ContactValidate.CreateContact;
using MensaApp.Infrastructure.Validations.ProductValidate.CreateProduct;
using MensaApp.Infrastructure.Validations.ProductValidate.UpdateProduct;
using MensaApp.Infrastructure.Validations.ProjectValidate;
using MensaApp.Infrastructure.Validations.SubCategoryValidate;
using Microsoft.Extensions.DependencyInjection;

namespace MensaApp.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAboutService, AboutManager>();
            serviceCollection.AddScoped<ICategoryService, CategoryManager>();
            serviceCollection.AddScoped<IContactService, ContactManager>();
            serviceCollection.AddScoped<IProductService, ProductManager>();
            serviceCollection.AddScoped<ISubCategoryService, SubCategoryManager>();
            serviceCollection.AddScoped<IProjectService, ProjectManager>();
        }
        public static void AddValidationServices(this IServiceCollection services)
        {
            services.AddControllers(options => options.Filters.Add<ValidationFilter>())
            .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateAboutValidation>())
            .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            services.AddControllers(options => options.Filters.Add<ValidationFilter>())
            .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<UpdateAboutValidation>())
            .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            services.AddControllers(options => options.Filters.Add<ValidationFilter>())
            .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateCategoryValidation>())
            .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            services.AddControllers(options => options.Filters.Add<ValidationFilter>())
            .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<UpdateCategoryValidation>())
            .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            services.AddControllers(options => options.Filters.Add<ValidationFilter>())
            .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateContactValidator>())
            .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            services.AddControllers()
            .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidation>())
            .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            services.AddControllers(options => options.Filters.Add<ValidationFilter>())
            .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<UpdateProductValidation>())
            .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            services.AddControllers(options => options.Filters.Add<ValidationFilter>())
            .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<SubCategoryValidation>())
            .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            services.AddControllers(options => options.Filters.Add<ValidationFilter>())
           .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<ProjectValidation>())
           .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
        }
    }
}
