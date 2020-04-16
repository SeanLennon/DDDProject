using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface IEmailSender
    {
        Task<SmtpStatusCode> SendAsync(string email, string message, string subject);
        Task<String> CreateMessageForgotPassword(string email, string token);
        Task<String> CreateWellcomeMessage(string name);
    }
}