using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces.Services
{
    public interface IUserService : IUserRepository
    {
        Task<String> AuthenticateAsync(string email, string password);
        Task<String> ForgotPasswordAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string newPassword, string token);
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        Task<IdentityResult> ChangeNameAsync(User user, string fullName);
    }
}