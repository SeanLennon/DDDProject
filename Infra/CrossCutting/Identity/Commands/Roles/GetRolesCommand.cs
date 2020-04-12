using System;
using System.Collections.Generic;
using Domain.Interfaces.Commands;

namespace Identity.Commands.Roles
{
    public class GetRolesCommand : ICommandResult
    {
        public List<String> Roles { get; set; }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}