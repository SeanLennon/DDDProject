using System.Collections.Generic;

namespace Domain.Interfaces.Commands.Users
{
    public interface IRegisterUserCommand
    {
        string UserName { get; set; }
        string FullName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string ConfirmPassword { get; set; }
        IList<string> Roles { get; set; }
    }
}