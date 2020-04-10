using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Domain.Resources;
using Microsoft.Extensions.Configuration;

namespace Identity.Services
{
    public static class EmailService
    {
        public static async Task<SmtpStatusCode> SendResetPasswordAsync(string email, string message, string subject)
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            using var client = new SmtpClient(config["EmailSettings:Server"])
            {
                Port = int.Parse(config["EmailSettings:Port"]),
                Credentials = new NetworkCredential(config["EmailSettings:From"], config["EmailSettings:Password"]),
                EnableSsl = true
            };

            using var mail = new MailMessage()
            {
                From = new MailAddress(config["EmailSettings:From"]),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mail.To.Add(email);
            try
            {
                await client.SendMailAsync(mail);
                return SmtpStatusCode.Ok;
            }
            catch (SmtpException)
            {
                return SmtpStatusCode.GeneralFailure;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}