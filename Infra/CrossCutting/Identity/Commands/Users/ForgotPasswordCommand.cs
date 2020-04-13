using Domain.Interfaces.Commands;

namespace Identity.Commands.Users
{
    public class ForgotPasswordCommand : ICommandResult
    {
        public string Email { get; set; }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}