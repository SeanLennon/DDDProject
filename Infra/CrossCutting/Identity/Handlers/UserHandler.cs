using System;
using System.Net.Mail;
using System.Text;
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
using Microsoft.AspNetCore.Identity;
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
        private ILoggerManager _logger;
        private UserManager<User> _userManager;

        public UserHandler(IUserService service, UserManager<User> userManager, ILoggerManager logger)
        {
            _service = service;
            _logger = logger;
            _userManager = userManager;
        }



        public async Task<ICommandResult> Handler(RegisterUserCommand command)
        {
            try
            {
                User user = new User(command.FullName, command.UserName, command.Email);
                IdentityResult result = await _service.InsertAsync(user, command.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddClaimsAsync(user, user.Claims);
                    await _userManager.AddToRolesAsync(user, command.Roles);

                    string message = await _service.CreateWellcomeMessage(user.FirstName);

                    // TODO: Refatorar para enviar email à uma fila em redis ou rabbitmq
                    /* var factory = new ConnectionFactory() { HostName = "localhost" };
                    using var connection = factory.CreateConnection();
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "email",
                                             durable: false,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                                             routingKey: "email",
                                             basicProperties: null,
                                             body: body);
                        _logger.Debug(message);
                    }; */
                    

                    SmtpStatusCode status = await _service.SendAsync(user.Email, message, subject: "Bem-vindo!");
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
                string message = await _service.CreateMessageForgotPassword(user.Email, token);
                SmtpStatusCode status = await _service.SendAsync(user.Email, message, "Reset Password");
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