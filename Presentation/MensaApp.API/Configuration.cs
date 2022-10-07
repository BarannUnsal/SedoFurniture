using Microsoft.Extensions.Configuration;

namespace MensaApp.Persistence
{
    static class Configuration
    {
        public static string ConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new();
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory()));
                configurationManager.AddJsonFile("appsettings.json");

                return configurationManager.GetConnectionString("MsSQL");
            }
        }
    }
}
