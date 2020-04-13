using System.ComponentModel.DataAnnotations;
using Domain.Interfaces.Commands;

namespace Identity.Commands.Users
{
    public class AuthenticateUserCommand : ICommandResult
    {
        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required, MaxLength(100)]
        public string Password { get; set; }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}