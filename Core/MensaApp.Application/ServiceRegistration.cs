using Microsoft.Extensions.DependencyInjection;

namespace MensaApp.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddHttpClient();
        }
    }
}
