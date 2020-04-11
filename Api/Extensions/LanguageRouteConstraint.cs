using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Api.Extensions
{
    public class LanguageRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext context, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (!context.Request.Headers.ContainsKey("Content-Language"))
                return false;

            string culture = context.Request.Headers
                .FirstOrDefault(x => x.Key == "Content-Language").Value;
            return culture == "en-US" || culture == "pt-BR";
        }
    }
}