using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface IEmailSend
    {
        Task SendWellComeEmailAsync(string message, string subject, string email);
    }
}