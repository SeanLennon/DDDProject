using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration
{
    public static class CorsConfig
    {
        public static void AddCorsConfig(this IServiceCollection services)
        {
            services.AddCors(x =>
            {
                x.AddPolicy("Default", c =>
                {
                    c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
        }
    }
}