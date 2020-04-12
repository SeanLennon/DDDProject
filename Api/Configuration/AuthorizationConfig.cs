using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration
{
    public static class AuthorizationConfig
    {
        public static void AddAuthorizationConfig(this IServiceCollection services)
        {
            services.AddAuthorization(x =>
            {
                x.AddPolicy("Default", p =>
                {
                    p.RequireRole("User");
                    p.RequireAssertion(c => c.User.IsInRole("User"));
                });
            });
        }
    }
}