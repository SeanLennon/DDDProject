using System;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface ITokenService
    {
        Task<String> GenerateToken(User user);
    }
}