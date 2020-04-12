using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Extensions;
using Domain.Interfaces.Commands;
using Domain.Interfaces.Handlers;
using Identity.Commands;
using Identity.Commands.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Handlers
{
    public class RoleHandler :
        IHandler<CreateRoleCommand>,
        IHandler<GetRoleCommand>,
        IHandler<GetRolesCommand>
    {
        private RoleManager<Role> _manager;

        public RoleHandler(RoleManager<Role> manager) => _manager = manager;

        public async Task<ICommandResult> Handler(CreateRoleCommand command)
        {
            try
            {
                Role role = new Role(command.Name);
                IdentityResult result = await _manager.CreateAsync(role);
                if (result.Succeeded)
                    return new CommandResult(true, "", result);
                return new CommandResult(false, "", result.Errors);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ICommandResult> Handler(GetRoleCommand command)
        {
            Role role = await _manager.FindByNameAsync(command.Name);
            if (role == null)
                return new CommandResult(false, "", null);
            return new CommandResult(true, "", role.Name);
        }

        public async Task<ICommandResult> Handler(GetRolesCommand command)
        {
            List<Role> roles = await _manager.Roles
                .AsNoTracking()
                .ToListAsync();

            if (roles == null)
                return new CommandResult(false, "", null);
            return new CommandResult(true, "", roles.Select(x => x.ToResponse()).ToList());
        }
    }
}