using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Data.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration) => _configuration = configuration;

        public Task<String> GenerateToken(User user)
        {
            return Task<String>.Run(() =>
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
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["AppSettings:Secret"])),
                        SecurityAlgorithms.HmacSha256Signature),
                    NotBefore = DateTime.Now
                };
                JwtSecurityTokenHandler tokenHadler = new JwtSecurityTokenHandler();
                return tokenHadler.WriteToken(tokenHadler.CreateToken(tokenDescriptor));
            });
        }
    }
}