using Data.Context;
using Identity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Setup
{
    public static class Database
    {
        public static void Setup(IServiceCollection services) =>
            services.AddDbContext<UserDbContext>(x =>
                x.UseNpgsql("Host=localhost; Port=5432; User ID=sean; Password=sean@123; Database=DDDProjectDb;", x =>
                    x.MigrationsAssembly("Api").SetPostgresVersion(0, 1)));
    }
}