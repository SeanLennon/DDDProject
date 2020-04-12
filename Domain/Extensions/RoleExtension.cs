using Domain.Entities;

namespace Domain.Extensions
{
    public static class RoleExtension
    {
        public static string ToName(this Role role) => role.Name;
    }
}