using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace Api.Extensions
{
    public class HeaderDataRequestCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            string culture = context.Request.Headers.FirstOrDefault(x => x.Key == "Content-Language").Value;
            if (culture == null) culture = "en-US";
            var provider = new ProviderCultureResult(culture, culture.ToLower());
            return Task.FromResult(provider);
        }
    }
}