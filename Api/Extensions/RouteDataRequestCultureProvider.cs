using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace Api.Extensions
{
    public class RouteDataRequestCultureProvider : RequestCultureProvider
    {
        public int IndexOfCulture;
        public int IndexOfUICulture;

        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            string culture = httpContext.Request.Path.Value.Split('/')[IndexOfCulture]?.ToString();
            string uiCulture = httpContext.Request.Path.Value.Split('/')[IndexOfUICulture]?.ToString().ToLower();

            var provider = new ProviderCultureResult(culture, uiCulture);
            return Task.FromResult(provider);
        }
    }
}