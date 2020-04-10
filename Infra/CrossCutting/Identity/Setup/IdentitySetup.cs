using Domain.Entities;
using Identity.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Setup
{
    public static class IdentitySetup
    {
        public static void Add(IServiceCollection services)
        {
            services.AddDefaultIdentity<User>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<UserDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 4;
                options.User.RequireUniqueEmail = true;
            });
        }
    }
}