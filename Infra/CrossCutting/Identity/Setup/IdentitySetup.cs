using System.Security.Claims;
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
            services.AddDefaultIdentity<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 4;
                options.User.RequireUniqueEmail = true;
                options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();
        }
    }
}