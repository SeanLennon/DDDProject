using System;

namespace Domain.Interfaces.Commands
{
    public interface ICommandResult
    {
        Boolean Succeeded { get; set; }
        String Message { get; set; }
        Object Data { get; set; }
    }
}