using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Extensions;
using Domain.Interfaces.Commands;
using Domain.Interfaces.Handlers;
using Domain.Interfaces.Services;
using Domain.Resources;
using Identity.Commands.Users;
using Identity.Models;
using Microsoft.AspNetCore.Identity;

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

        public UserHandler(IUserService service) => _service = service;



        public async Task<ICommandResult> Handler(RegisterUserCommand command)
        {
            try
            {
                User user = new User(fullName: command.FullName, username: command.UserName, email: command.Email);
                IdentityResult result = await _service.InsertAsync(user, command.Password);
                if (result.Succeeded)
                {
                    // TODO: Send wellcome email
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
                string code = await _service.ForgotPasswordAsync(command.Email);
                if (code == null)
                    return new CommandResult(false, Messages.FORGOT_PASSWORD_FAILED, null);

                return new CommandResult(true, Messages.FORGOT_PASSWORD_SUCCESS, code);
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