using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Interfaces.Services
{
    public interface ITokenService
    {
        Task<Token> GenerateToken(User user);
    }
}