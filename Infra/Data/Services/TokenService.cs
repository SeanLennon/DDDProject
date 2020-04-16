using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Data.Helpers;
using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Data.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration) => _configuration = configuration;

        public Task<Token> GenerateToken(User user)
        {
            AppSettings app = _configuration.GetSection("AppSettings").Get<AppSettings>();
            return Task<Token>.Run(() =>
            {
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(user.Claims),
                    Expires = DateTime.Now.AddHours(6),
                    SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(app.Secret)),
                        SecurityAlgorithms.HmacSha256Signature),
                    NotBefore = DateTime.Now,
                    Audience = app.Audience,
                    Issuer = app.Issuer
                };
                JwtSecurityTokenHandler tokenHadler = new JwtSecurityTokenHandler();
                return new Token(tokenHadler.WriteToken(tokenHadler.CreateToken(tokenDescriptor)));
            });
        }
    }
}