using System.ComponentModel.DataAnnotations;
using Domain.Interfaces.Commands;

namespace Identity.Models
{
    public class ChangeNameCommand : ICommandResult
    {
        [EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(200)]
        public string FullName { get; set; }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}