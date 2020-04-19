using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Interfaces.Commands;
using Domain.Interfaces.Commands.Users;

namespace Identity.Commands.Users
{
    public class RegisterUserCommand : ICommandResult, IRegisterUserCommand
    {
        [Required(ErrorMessage = "Nome de usuário é obrigatório."), MaxLength(100)]
        public string UserName { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string Password { get; set; }

        [Required, Compare("Password"), MaxLength(100)]
        public string ConfirmPassword { get; set; }
        public IList<string> Roles { get; set; }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}