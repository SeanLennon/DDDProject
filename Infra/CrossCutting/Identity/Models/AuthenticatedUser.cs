using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Identity.Models
{
    public class AuthenticatedUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AuthenticatedUser(IHttpContextAccessor accessor) => _accessor = accessor;

        public string UserId => GetClaims(ClaimTypes.NameIdentifier);
        public string Email => GetClaims(ClaimTypes.Email);
        public string Name => GetClaims(ClaimTypes.GivenName);
        public string UserName => GetClaims(ClaimTypes.Name);
        public string Role => GetClaims(ClaimTypes.Role);
        public string Token => _accessor.HttpContext.Request.Query?.FirstOrDefault(x => x.Key == "token").Value;

        public string GetClaims(string type)
            => _accessor.HttpContext.User.FindFirstValue(type);
    }
}