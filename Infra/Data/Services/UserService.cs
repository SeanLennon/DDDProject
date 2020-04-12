using System;
using System.Threading.Tasks;
using Data.Context;
using Data.Repositories;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Data.Services
{
    public class UserService : UserRepository, IUserService
    {
        private IUserRepository _userRepository;
        private ITokenService _tokenService;

        public UserService(UserManager<User> manager, AppDbContext context, IUserRepository userRepository, ITokenService tokenService)
            : base(manager, context)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }


        public async Task<String> AuthenticateAsync(string email, string password)
        {
            User user = await _userRepository.GetByEmailAsync(email);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                Token token = await _tokenService.GenerateToken(user);
                return token.AccessToken;
            }
            return null;
        }

        public async Task<string> ForgotPasswordAsync(User user)
            => await _userManager.GeneratePasswordResetTokenAsync(user);

        public async Task<IdentityResult> ResetPasswordAsync(User user, string newPassword, string token)
            => await _userManager.ResetPasswordAsync(user, token, newPassword);

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
            => await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);


        public async Task<IdentityResult> ChangeNameAsync(User user, string fullName)
        {
            user.ChangeName(fullName);
            return await _userRepository.UpdateAsync(user);
        }
    }
}