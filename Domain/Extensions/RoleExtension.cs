using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.Extensions
{
    public static class RoleExtension
    {
        public static string ToName(this IdentityRole role) => role.Name;

        public static object ToResponse(this IdentityRole role) => new { Id = role.Id, Name = role.Name };
    }
}