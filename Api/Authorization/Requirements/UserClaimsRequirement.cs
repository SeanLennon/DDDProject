using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization.Requirements
{
    public class UserClaimsRequirement : IAuthorizationRequirement
    {
        public UserClaimsRequirement(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}