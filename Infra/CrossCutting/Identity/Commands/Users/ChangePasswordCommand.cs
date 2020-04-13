using System.ComponentModel.DataAnnotations;
using Domain.Interfaces.Commands;
using Identity.Models;

namespace Identity.Commands.Users
{
    public class ChangePasswordCommand : ICommandResult
    {
        public ChangePasswordCommand(AuthenticatedUser user) => Email = user.Email;

        [EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string OldPassword { get; set; }

        [Required, MaxLength(100)]
        public string NewPassword { get; set; }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}