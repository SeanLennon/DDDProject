using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Context;
using Data.Helpers;
using Data.Repositories;
using Domain.Entities;
using Domain.Interfaces.Managers;
using Domain.Interfaces.Services;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;

namespace Data.Services
{
    public class UserService : UserRepository, IUserService
    {
        private readonly EmailSettings _settings;
        private ITokenService _tokenService;
        private ILoggerManager _logger;

        public UserService(
            UserManager<User> manager,
            AppDbContext context,
            ITokenService tokenService,
            ILoggerManager logger,
            IConfiguration config
        ) : base(manager, context)
        {
            _tokenService = tokenService;
            _logger = logger;
            _settings = config.GetSection("EmailSettings").Get<EmailSettings>();
        }


        public async Task<String> AuthenticateAsync(string email, string password)
        {
            _logger?.Info("Obtendo o usuário.");
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger?.Warn("Usuário não encontrado na base de dados.");
                return null;
            }
            _logger?.Info("Usuário foi encontrado.");

            _logger?.Info("validando a senha.");
            if (await _userManager.CheckPasswordAsync(user, password))
            {
                _logger?.Info("Senha é válida.");
                _logger?.Info("Gerando Jwt Token.");

                IList<string> roles = await _userManager.GetRolesAsync(user);
                IList<Claim> claims = await _userManager.GetClaimsAsync(user);
                claims.Add(new Claim(ClaimTypes.Role, roles.Join(", ")));
                user.AddClaims(claims);

                Token token = await _tokenService.GenerateToken(user);
                if (token != null)
                {
                    _logger?.Info("Token gerado com sucesso.");
                    _logger?.Info("Retornando token.");
                    return token.AccessToken;
                }
                _logger?.Info("Falha ao gerar token.");
                return null;
            }
            _logger?.Warn("Senha não é válida para esse usuário.");
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
            return await _userManager.UpdateAsync(user);
        }


        public Task<SmtpStatusCode> SendAsync(string email, string message, string subject)
        {
            return Task<SmtpStatusCode>.Run(async () =>
            {
                using var smtp = new SmtpClient(_settings.Server)
                {
                    Port = _settings.Port,
                    Credentials = new NetworkCredential(_settings.From, _settings.Password),
                    EnableSsl = true
                };

                using var mail = new MailMessage()
                {
                    From = new MailAddress(email),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true,
                };
                mail.To.Add(email);
                try
                {
                    await smtp.SendMailAsync(mail);
                    return await Task.FromResult(SmtpStatusCode.Ok);
                }
                catch (SmtpException)
                {
                    return await Task.FromResult(SmtpStatusCode.GeneralFailure);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            });
        }

        public Task<String> CreateMessageForgotPassword(string email, string token)
            => Task<String>.Run(() => string.Format("Clique no link para redefinir sua senha: <a href=\"https://api.localhost:5001/reset-password?email={0}&token={1}\">{1}</a><br><p>Caso não tenha sido você, ignore esse E-mail.</p>", email, token));

        public Task<String> CreateWellcomeMessage(string name)
            => Task<String>.Run(() => String.Format("Wellcome {0}! <br> <div style=\"background-color:silver;\">Have you been registered successfully.</br>", name));
    }
}