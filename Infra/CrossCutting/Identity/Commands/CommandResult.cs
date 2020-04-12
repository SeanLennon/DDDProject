using System;
using Domain.Interfaces.Commands;

namespace Identity.Commands
{
    public class CommandResult : ICommandResult
    {
        public CommandResult(bool succeeded, string message, object data)
        {
            Succeeded = succeeded;
            Message = message;
            Data = data;
        }

        public Boolean Succeeded { get; set; }
        public String Message { get; set; }
        public Object Data { get; set; }
    }
}