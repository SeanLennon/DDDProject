using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository : IDisposable
    {
        Task<IdentityResult> InsertAsync(User user, string password);
        Task<IdentityResult> UpdateAsync(User user);
        /* Task<IdentityResult> AddClaimsAsync(User user, IEnumerable<Claim> claims);
        Task<IdentityResult> AddRolesAsync(User user, IEnumerable<string> roles); */

        Task<User> GetByEmailAsync(string email);
    }
}