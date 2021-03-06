using System.ComponentModel.DataAnnotations;
using Domain.Interfaces.Commands;
using Identity.Models;

namespace Identity.Commands.Users
{
    public class ProfileUserCommand : ICommandResult
    {
        public ProfileUserCommand(AuthenticatedUser user) => Email = user.Email;

        [EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}