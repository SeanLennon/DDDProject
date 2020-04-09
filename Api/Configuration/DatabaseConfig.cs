using Data.Setup;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration
{
    public static class DatabaseConfig
    {
        public static void AddDatabaseConfig(this IServiceCollection services) => Database.Setup(services);
    }
}