using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager) => _userManager = userManager;


        public async Task<IdentityResult> InsertAsync(User user, string password)
            => await _userManager.CreateAsync(user, password);

        public async Task<IdentityResult> UpdateAsync(User user)
            => await _userManager.UpdateAsync(user);

        public async Task<User> GetByEmailAsync(string email)
            => await _userManager.FindByEmailAsync(email);


        public void Dispose()
        {
            _userManager.Dispose();
            GC.SuppressFinalize(true);
        }
    }
}