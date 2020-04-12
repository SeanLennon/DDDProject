using System;
using System.Threading.Tasks;
using Data.Context;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected UserManager<User> _userManager;
        protected AppDbContext _context;

        public UserRepository(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


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