using Api.Authorization.Requirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration
{
    public static class AuthorizationConfig
    {
        public static void AddAuthorizationConfig(this IServiceCollection services)
        {
            services.AddAuthorization(x =>
            {
                x.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build();

                x.AddPolicy("NoneOnly", x =>
                {
                    x.Requirements.Add(new UserRolesRequirement(new string[] { "None" }));
                });

                x.AddPolicy("ManagerOnly", x =>
                {
                    x.Requirements.Add(new UserRolesRequirement(new string[] { "Manager" }));
                });

                x.AddPolicy("AdminOnly", x =>
                {
                    x.Requirements.Add(new UserRolesRequirement(new string[] { "Admin" }));
                });

                x.AddPolicy("UserOnly", x =>
                {
                    x.Requirements.Add(new UserRolesRequirement(new string[] { "User" }));
                });
            });
        }
    }
}