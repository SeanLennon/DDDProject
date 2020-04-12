using System;
using System.Collections.Generic;
using System.Globalization;
using Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration
{
    public static class GlobalizationConfig
    {
        public static void AddGlobalizationConfig(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddLocalization(x => x.ResourcesPath = "Resources");

            services.Configure<RouteOptions>(x =>
            {
                x.ConstraintMap.Add("Content-Language", typeof(LanguageRouteConstraint));
            });

            services.Configure<RequestLocalizationOptions>(x =>
            {
                var cultures = new List<CultureInfo>()
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("pt-BR")
                };
                x.DefaultRequestCulture = new RequestCulture("en-US", "en-us");
                x.SupportedCultures = cultures;
                x.SupportedUICultures = cultures;
                x.RequestCultureProviders = new[]
                {
                    new HeaderDataRequestCultureProvider()
                };
                x.SetDefaultCulture("en-US");
            });
        }
    }
}