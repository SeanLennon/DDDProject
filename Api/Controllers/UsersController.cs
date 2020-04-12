using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Handlers;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Api.Controllers
{
    [Authorize(Policy = "Default"), Route("users"), ApiVersion("1.0"), ApiController]
    public class UsersController : ControllerBase
    {
        private UserHandler _handler;
        private IStringLocalizer<UsersController> _localizer;

        public UsersController(IStringLocalizer<UsersController> localizer, UserHandler handler)
        {
            _handler = handler;
            _localizer = localizer;
        }


        // GET: https://api.localhost:5001/users/authenticate
        [AllowAnonymous]
        [HttpGet, Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromForm]AuthenticateUserCommand command)
        {
            var result = await _handler.Handler(command);
            if (result.Succeeded)
            {
                result.Message = _localizer["USER_AUTHENTICATE_SUCCESS"].Value;
                return Ok(result);
            }
            result.Message = _localizer["USER_AUTHENTICATE_FAILED"].Value;
            return BadRequest(result);
        }



        // GET: https://api.localhost:5001/users/forgot-password
        [AllowAnonymous]
        [HttpGet, Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromForm]ForgotPasswordCommand command)
        {
            var result = await _handler.Handler(command);
            if (result.Succeeded)
            {
                result.Message = _localizer["FORGOT_PASSWORD_SUCCESS"].Value;
                return Ok(result);
            }
            result.Message = _localizer["FORGOT_PASSWORD_FAILED"].Value;
            return BadRequest(result);
        }



        // PATCH: https://api.localhost:5001/users/reset-password/email=exemplo@exemplo.com&token=CfDJ8DW0ycF3tRJEv5qbVtaA2vc4
        [AllowAnonymous]
        [HttpPatch, Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromServices]ResetPasswordCommand command)
        {
            var result = await _handler.Handler(command);
            if (result.Succeeded)
            {
                result.Message = _localizer["RESET_PASSWORD_SUCCESS"].Value;
                return Ok(result);
            }
            result.Message = _localizer["RESET_PASSWORD_FAILED"].Value;
            return BadRequest(result);
        }



        // PATCH: https://api.localhost:5001/users/change-password
        [Authorize(Roles = "User")]
        [HttpPatch, Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromForm]ChangePasswordCommand command)
        {
            var result = await _handler.Handler(command);
            if (result.Succeeded)
            {
                result.Message = _localizer["CHANGE_PASSWORD_SUCCESS"].Value;
                return Ok(result);
            }
            result.Message = _localizer["CHANGE_PASSWORD_FAILED"].Value;
            return BadRequest(result);
        }



        // POST: https://api.localhost:5001/users
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm]RegisterUserCommand command)
        {
            var result = await _handler.Handler(command);
            if (result.Succeeded)
            {
                result.Message = _localizer["USER_REGISTER_SUCCESS"].Value;
                return Created($"https://api.localhost:5001/users/profile", result);
            }
            result.Message = _localizer["USER_REGISTER_FAILED"].Value;
            return BadRequest(result);
        }



        // GET: https://api.localhost:5001/users/profile
        [Authorize(Roles = "User")]
        [HttpGet, Route("profile")]
        public async Task<IActionResult> Profile([FromServices]ProfileUserCommand command)
        {
            var result = await _handler.Handler(command);
            if (result.Succeeded)
            {
                result.Message = _localizer["FIND_PROFILE_SUCCESS"].Value;
                return Ok(result);
            }
            result.Message = _localizer["FIND_PROFILE_FAILED"].Value;
            return NotFound(result);
        }



        // PATCH: https://api.localhost:5001/users/change-name
        [Authorize(Roles = "User")]
        [HttpPatch, Route("change-name")]
        public async Task<IActionResult> ChangeName([FromForm]ChangeNameCommand command)
        {
            var result = await _handler.Handler(command);
            if (result.Succeeded)
            {
                result.Message = _localizer["CHANGE_NAME_SUCCESS"].Value;
                return Ok(result);
            }
            result.Message = _localizer["CHANGE_NAME_FAILED"].Value;
            return BadRequest(result);
        }
    }
}