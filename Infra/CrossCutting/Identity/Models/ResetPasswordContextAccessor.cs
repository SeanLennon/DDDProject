using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Identity.Models
{
    public class ResetPasswordContextAccessor
    {
        private readonly IHttpContextAccessor _accessor;

        public ResetPasswordContextAccessor(IHttpContextAccessor accessor) => _accessor = accessor;

        public string Email => GetQueryData("email");
        public string NewPassword => GetFormData("NewPassword");
        public string Token => GetQueryData("token");

        public string GetQueryData(string query)
            => _accessor.HttpContext.Request.Query?.FirstOrDefault(x => x.Key == query).Value;

        public string GetFormData(string data)
            => _accessor.HttpContext.Request.Form?.FirstOrDefault(x => x.Key == data).Value;
    }
}