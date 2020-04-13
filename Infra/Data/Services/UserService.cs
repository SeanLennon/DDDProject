using System;
using System.Threading.Tasks;
using Data.Context;
using Data.Repositories;
using Domain.Entities;
using Domain.Interfaces.Managers;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace Data.Services
{
    public class UserService : UserRepository, IUserService
    {
        private IUserRepository _userRepository;
        private ITokenService _tokenService;
        private ILoggerManager _logger;

        public UserService(UserManager<User> manager, AppDbContext context, IUserRepository userRepository, ITokenService tokenService, ILoggerManager logger, IHostEnvironment env)
            : base(manager, context)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            // if (env.IsProduction())
                _logger = logger;
        }


        public async Task<String> AuthenticateAsync(string email, string password)
        {
            _logger?.LogInfo("Obtendo o usuário.");
            User user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                _logger?.LogWarn("Usuário não encontrado na base de dados.");
                return null;
            }
            _logger?.LogInfo("Usuário foi encontrado.");

            _logger?.LogInfo("validando a senha.");
            if (await _userManager.CheckPasswordAsync(user, password))
            {
                _logger?.LogInfo("Senha é válida.");
                _logger?.LogInfo("Gerando Jwt Token.");
                Token token = await _tokenService.GenerateToken(user);
                if(token != null)
                {
                    _logger?.LogInfo("Token gerado com sucesso.");
                    _logger?.LogInfo("Retornando token.");
                    return token.AccessToken;
                }
                _logger?.LogInfo("Falha ao gerar token.");
                return null;
            }
            _logger?.LogWarn("Senha não é válida para esse usuário.");
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