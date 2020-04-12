using Domain.Entities;

namespace Domain.Extensions
{
    public static class RoleExtension
    {
        public static string ToName(this Role role) => role.Name;

        public static object ToResponse(this Role role) => new { Id = role.Id, Name = role.Name };
    }
}