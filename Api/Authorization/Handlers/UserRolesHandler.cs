using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization.Handlers
{
    public class UserRolesHandler : AuthorizationHandler<UserRolesRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRolesRequirement requirement)
        {
            string roles = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

            if (roles != null && requirement.Roles.Select(x => roles.Contains(x)).Contains(true))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}