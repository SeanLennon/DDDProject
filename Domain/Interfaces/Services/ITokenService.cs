using System;
using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface ITokenService
    {
        User GenerateToken(User user);
    }
}