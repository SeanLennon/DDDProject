using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Extensions
{
    public static class UserExtension
    {
        //
        // Summary:
        //     Generate Json Web Token.
        // Returns:
        //     The Token Jwt.
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     type or value in user is null.
        public static String Jwt(this User user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.GivenName, user.ToString()),
                    new Claim("full_name", user.FullName),
                    new Claim("first_name", user.FirstName),
                    new Claim("last_name", user.LastName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, "User")
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes("de161a0d-984f-4249-9705-5bdc6e02c548")),
                        SecurityAlgorithms.HmacSha256Signature),
                NotBefore = DateTime.Now
            };
            JwtSecurityTokenHandler tokenHadler = new JwtSecurityTokenHandler();
            return tokenHadler.WriteToken(tokenHadler.CreateToken(tokenDescriptor));
        }

        public static Object Response(this User user)
            => new
            {
                Id = user.Id,
                Name = user.ToString(),
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName
            };
    }
}