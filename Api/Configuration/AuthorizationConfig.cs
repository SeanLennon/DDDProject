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

                x.AddPolicy("Default", p =>
                {
                    p.RequireRole("User");
                    p.RequireAssertion(c => c.User.IsInRole("User"));
                });
            });
        }
    }
}