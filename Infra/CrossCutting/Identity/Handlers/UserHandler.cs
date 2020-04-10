using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Extensions;
using Domain.Interfaces.Commands;
using Domain.Interfaces.Handlers;
using Domain.Interfaces.Services;
using Domain.Resources;
using Identity.Commands.Users;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

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
        private IStringLocalizer<UserHandler> _localizer;

        public UserHandler(IUserService service, IStringLocalizer<UserHandler> localizer)
        {
            _service = service;
            _localizer = localizer;
        }



        public async Task<ICommandResult> Handler(RegisterUserCommand command)
        {
            try
            {
                User user = new User(command.FullName, command.UserName, command.Email);
                IdentityResult result = await _service.InsertAsync(user, command.Password);
                if (result.Succeeded)
                {
                    string message = string.Format("<strong><b>Wellcome {0}! Registered successfully.</b></strong>", command.FullName);
                    SmtpStatusCode status = await EmailService.SendAsync(command.Email, message, $"Wellcome {command.FullName}");
                    if (status == SmtpStatusCode.GeneralFailure)
                        return new CommandResult(false, Messages.FORGOT_PASSWORD_FAILED, null);

                    return new CommandResult(true, Messages.USER_REGISTER_SUCCESS, user.Response());
                }
                return new CommandResult(false, Messages.USER_REGISTER_FAILED, result.Errors);
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
                string token = await _service.AuthenticateAsync(command.Email, command.Password);
                if (token == null)
                    return new CommandResult(false, Messages.USER_AUTHENTICATE_FAILED, null);
                return new CommandResult(true, Messages.USER_AUTHENTICATE_SUCCESS, token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                string message = string.Format("Clique no link para redefinir sua senha: <a href=\"https://localhost:5001/reset-password?email={0}&token={1}\">{1}</a><br><p>Caso não tenha sido você ignore esse E-mail.</p>", user.Email, token);
                SmtpStatusCode status = await EmailService.SendAsync(user.Email, message, "Reset Password");
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