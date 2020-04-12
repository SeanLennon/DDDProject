using System;
using Api.Helpers;
using Data.Context;
using Identity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Api.Configuration
{
    public static class DatabaseConfig
    {
        public static void AddDatabaseConfig(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            DatabaseSettings database = configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
            var conn = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString("AppConnectionString"))
            {
                Database = database.Name,
                Username = database.Username,
                Password = database.Password
            }.ConnectionString;

            services.AddDbContext<UserDbContext>(x =>
                x.UseNpgsql(conn, x =>
                    x.MigrationsAssembly("Api").SetPostgresVersion(0, 1)));

            services.AddDbContext<AppDbContext>(x =>
                x.UseNpgsql(conn, x =>
                    x.MigrationsAssembly("Api").SetPostgresVersion(0, 1)));
        }
    }
}