using System;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository : IDisposable
    {
        Task<IdentityResult> InsertAsync(User user, string password);
        Task<IdentityResult> UpdateAsync(User user);

        Task<User> GetByEmailAsync(string email);
    }
}