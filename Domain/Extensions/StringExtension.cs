using System.Linq;

namespace Domain.Extensions
{
    public static class StringExtension
    {
        public static string ToFirstName(this string str) => str.Split(' ').FirstOrDefault();
        public static string ToLastName(this string str) => str.Split(' ').LastOrDefault();
        public static string ToUserName(this string str) => $"@{str.ToLower()}";
    }
}