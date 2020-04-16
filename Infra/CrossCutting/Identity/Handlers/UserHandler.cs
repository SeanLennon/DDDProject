using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Extensions;
using Domain.Interfaces.Commands;
using Domain.Interfaces.Handlers;
using Domain.Interfaces.Managers;
using Domain.Interfaces.Services;
using Domain.Resources;
using Identity.Commands;
using Identity.Commands.Users;
using Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Identity.Handlers
{
    public class UserHandler :
        IHandler<RegisterUserCommand>,
        IHandler<AuthenticateUserCommand>,
        IHandler<ForgotPasswordCommand>,
        IHandler<ResetPasswordCommand>,
        IHandler<ChangePasswordCommand>,
        IHandler<ProfileUserCommand>,
        IHandler<ChangeNameCommand>
    {
        private IUserService _service;
        private IConfiguration _config;
        private ILoggerManager _logger;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;

        private List<IdentityError> _errors;

        public UserHandler(IUserService service, UserManager<User> userManager, IConfiguration config, ILoggerManager logger, IHostEnvironment env, RoleManager<IdentityRole> roleManager)
        {
            _service = service;
            _config = config;
            // if (env.IsProduction())
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }



        public async Task<ICommandResult> Handler(RegisterUserCommand command)
        {
            // List<IdentityError> errors = new List<IdentityError>();
            try
            {
                User user = new User(command.FullName, command.UserName, command.Email);
                IdentityResult result = await _service.InsertAsync(user, command.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddClaimsAsync(user, user.Claims);
                    await _userManager.AddToRolesAsync(user, command.Roles.Select(x => x.Name));

                    string message = EmailService.CreateWellcomeMessage(user.FirstName);

                    // TODO: Refatorar para enviar email à uma fila em redis ou rabbitmq
                    SmtpStatusCode status = await EmailService.SendAsync(command.Email, message, subject: "Wellcome", config: _config);
                    if (status == SmtpStatusCode.GeneralFailure)
                        return new CommandResult(false, Messages.FORGOT_PASSWORD_FAILED, null);

                    return new CommandResult(true, Messages.USER_REGISTER_SUCCESS, user.Response());
                }
                return new CommandResult(false, Messages.USER_REGISTER_FAILED, result.Errors);
            }
            catch (SmtpException ex)
            {
                throw new SmtpException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ICommandResult> Handler(AuthenticateUserCommand command)
        {
            try
            {
                _logger?.Info("Autenticando usuário.");
                string token = await _service.AuthenticateAsync(command.Email, command.Password);
                if (token == null)
                {
                    _logger?.Warn("Usuário não autenticado.");
                    return new CommandResult(false, Messages.USER_AUTHENTICATE_FAILED, null);
                }
                _logger?.Info("Usuário autenticado com sucesso.");
                return new CommandResult(true, Messages.USER_AUTHENTICATE_SUCCESS, token);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex.Message.ToLower());
                // return new CommandResult(false, "Internal Server Error", null);
                throw ex;
            }
        }

        public async Task<ICommandResult> Handler(ForgotPasswordCommand command)
        {
            try
            {
                User user = await _service.GetByEmailAsync(command.Email);
                if (user == null)
                    return new CommandResult(false, Messages.USER_NOT_FOUND, null);

                string token = await _service.ForgotPasswordAsync(user);
                string message = EmailService.CreateMessageForgotPassword(user.Email, token);
                SmtpStatusCode status = await EmailService.SendAsync(user.Email, message, "Reset Password", _config);
                if (status == SmtpStatusCode.GeneralFailure)
                    return new CommandResult(false, Messages.FORGOT_PASSWORD_FAILED, null);

                return new CommandResult(true, Messages.FORGOT_PASSWORD_SUCCESS, null);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ICommandResult> Handler(ResetPasswordCommand command)
        {
            try
            {
                User user = await _service.GetByEmailAsync(command.Email);
                if (user == null)
                    return new CommandResult(false, Messages.USER_NOT_FOUND, null);

                IdentityResult result = await _service.ResetPasswordAsync(user, command.NewPassword, command.Token);
                if (result.Succeeded)
                    return new CommandResult(true, Messages.RESET_PASSWORD_SUCCESS, null);
                return new CommandResult(false, Messages.RESET_PASSWORD_FAILED, result.Errors);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ICommandResult> Handler(ChangePasswordCommand command)
        {
            try
            {
                User user = await _service.GetByEmailAsync(command.Email);
                if (user == null)
                    return new CommandResult(false, Messages.USER_NOT_FOUND, null);

                IdentityResult result = await _service.ChangePasswordAsync(user, command.OldPassword, command.NewPassword);
                if (result.Succeeded)
                    return new CommandResult(true, Messages.CHANGE_PASSWORD_SUCCESS, null);
                return new CommandResult(false, Messages.CHANGE_PASSWORD_FAILED, result.Errors);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ICommandResult> Handler(ProfileUserCommand command)
        {
            try
            {
                User user = await _service.GetByEmailAsync(command.Email);
                if (user == null)
                    return new CommandResult(false, Messages.USER_NOT_FOUND, null);
                return new CommandResult(true, Messages.USER_FOUND, user.Response());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ICommandResult> Handler(ChangeNameCommand command)
        {
            try
            {
                User user = await _service.GetByEmailAsync(command.Email);
                if (user == null)
                    return new CommandResult(false, Messages.USER_NOT_FOUND, null);

                IdentityResult result = await _service.ChangeNameAsync(user, command.FullName);
                if (result.Succeeded)
                    return new CommandResult(true, Messages.CHANGE_NAME_SUCCESS, result);
                return new CommandResult(false, Messages.CHANGE_NAME_FAILED, result.Errors);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}