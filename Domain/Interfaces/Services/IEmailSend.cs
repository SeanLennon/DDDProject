using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IEmailSend
    {
        Task SendWellComeEmailAsync(string message, string subject, string email);
        string CreateEmailMessage(string message, string email, string token);
    }
}