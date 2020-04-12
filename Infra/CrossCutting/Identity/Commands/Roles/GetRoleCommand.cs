using Domain.Interfaces.Commands;

namespace Identity.Commands.Roles
{
    public class GetRoleCommand : ICommandResult
    {
        public string Name { get; set; }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}