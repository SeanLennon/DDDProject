using System.Collections.Generic;
using System.Globalization;
using Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration
{
    public static class CultureConfig
    {
        public static void AddLocalizationConfig(this IServiceCollection services)
        {
            services.AddLocalization(x => x.ResourcesPath = "Resources");
            // services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(x =>
            {
                var cultures = new List<CultureInfo>()
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("pt-BR")
                };
                x.DefaultRequestCulture = new RequestCulture("en-US", "en-US");
                x.SupportedCultures = cultures;
                x.SupportedUICultures = cultures;
                x.RequestCultureProviders = new[]
                {
                    new RouteDataRequestCultureProvider
                    {
                        IndexOfCulture = 1,
                        IndexOfUICulture = 1
                    }
                };
                x.SetDefaultCulture("en-US");
            });

            services.Configure<RouteOptions>(x =>
            {
                x.ConstraintMap.Add("Culture", typeof(LanguageRouteConstraint));
            });
        }
    }
}