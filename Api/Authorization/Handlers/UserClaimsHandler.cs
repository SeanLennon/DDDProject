using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization.Handlers
{
    public class UserClaimsHandler : AuthorizationHandler<UserClaimsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserClaimsRequirement requirement)
        {
            var claims = context.User.Claims.FirstOrDefault(x => x.Type == requirement.Name)?.Value;
            if (claims != null && claims.Contains(requirement.Name))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}