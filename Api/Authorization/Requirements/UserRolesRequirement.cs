using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization.Requirements
{
    public class UserRolesRequirement : IAuthorizationRequirement
    {
        public UserRolesRequirement(string[] roles)
        {
            Roles = roles;
        }

        public string[] Roles { get; set; }
    }
}