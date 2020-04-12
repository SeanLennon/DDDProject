using System.Threading.Tasks;
using Identity.Commands.Roles;
using Identity.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Api.Controllers
{
    [Authorize(Policy = "Default"), Route("roles"), ApiVersion("1.0"), ApiController]
    public class RolesController : ControllerBase
    {
        private RoleHandler _handler;
        private IStringLocalizer<RolesController> _localizer;

        public RolesController(RoleHandler handler, IStringLocalizer<RolesController> localizer)
        {
            _handler = handler;
            _localizer = localizer; ;
        }


        [HttpPost]
        public async Task<IActionResult> Get([FromForm]CreateRoleCommand command)
        {
            var result = await _handler.Handler(command);
            if (result.Succeeded)
            {
                result.Message = _localizer.GetString("ROLE_CREATED_SUCCESS").Value;
                return Ok(result);
            }
            result.Message = _localizer.GetString("ROLE_CREATED_FAILED").Value;
            return NotFound(result);
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get([FromForm]GetRoleCommand command)
        {
            var result = await _handler.Handler(command);
            if (result.Succeeded)
            {
                result.Message = _localizer.GetString("ROLE_FOUND").Value;
                return Ok(result);
            }
            result.Message = _localizer.GetString("ROLE_NOT_FOUND").Value;
            return NotFound(result);
        }

        [AllowAnonymous]
        [HttpGet, Route("all")]
        public async Task<IActionResult> GetAll([FromForm]GetRolesCommand command)
        {
            var result = await _handler.Handler(command);
            if (result.Succeeded)
            {
                result.Message = _localizer.GetString("ROLE_FOUND").Value;
                return Ok(result);
            }
            result.Message = _localizer.GetString("ROLE_NOT_FOUND").Value;
            return NotFound(result);
        }
    }
}