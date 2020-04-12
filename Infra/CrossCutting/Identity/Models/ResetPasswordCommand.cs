using System.ComponentModel.DataAnnotations;
using System.Linq;
using Domain.Interfaces.Commands;
using Microsoft.AspNetCore.Http;

namespace Identity.Models
{
    public class ResetPasswordCommand : ICommandResult
    {
        private ResetPasswordContextAccessor _accessor;

        public ResetPasswordCommand(ResetPasswordContextAccessor accessor) => _accessor = accessor;

        [EmailAddress, MaxLength(100)]
        public string Email { get => _accessor.Email; }

        [Required, MaxLength(100), DataType(DataType.Password)]
        public string NewPassword { get => _accessor.NewPassword; }

        [Required]
        public string Token { get => _accessor.Token; }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public override string ToString() => Email;
    }
}