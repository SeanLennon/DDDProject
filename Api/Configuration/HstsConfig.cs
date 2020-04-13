using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration
{
    public static class HstsConfig
    {
        public static void AddHstsConfig(this IServiceCollection services)
        {
            services.AddHsts(options =>
            {
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });

            services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = false;
            });
        }
    }
}