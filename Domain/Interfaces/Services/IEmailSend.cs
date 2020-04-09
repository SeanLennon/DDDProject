using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface IEmailSend
    {
        Task Send(string message, string subject, string to);
    }
}