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

        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            string culture = context.Request.Path.Value.Split('/')[IndexOfCulture]?.ToString();
            string uiCulture = context.Request.Path.Value.Split('/')[IndexOfUICulture]?.ToString().ToLower();

            var provider = new ProviderCultureResult(culture, uiCulture);
            return Task.FromResult(provider);
        }
    }
}