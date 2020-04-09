using System.ComponentModel.DataAnnotations;
using Domain.Interfaces.Commands;

namespace Identity.Models
{
    public class ResetPasswordCommand : ICommandResult
    {
        [EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(100)]
        public string NewPassword { get; set; }

        [Required, Compare("NewPassword"), MaxLength(100)]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }


        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public override string ToString() => Email;
    }
}