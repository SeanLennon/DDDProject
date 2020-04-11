using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Data.Services
{
    public static class EmailService
    {
        public static Task SendEmailResetPassword(string email, string message, string subject)
        {
            return Task.Run(() =>
            {
                IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables().Build();

                using var smtp = new SmtpClient(config["Server"])
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
                    IsBodyHtml = true,
                };
                mail.To.Add(email);
                try
                {
                    return smtp.SendMailAsync(mail);
                }
                catch (ArgumentNullException ex)
                {
                    throw new ArgumentNullException(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            });
        }
    }
}