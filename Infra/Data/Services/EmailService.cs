using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Data.Helpers;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Data.Services
{
    public class EmailService : IEmailSender
    {
        private readonly EmailSettings _settings;

        public EmailService(IConfiguration config) => _settings = config.GetSection("EmailSettings").Get<EmailSettings>();

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