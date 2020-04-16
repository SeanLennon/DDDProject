using System.Threading.Tasks;
using Identity.Commands.Users;
using Identity.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Api.Controllers
{
    [Authorize, Route("users"), ApiVersion("1.0"), ApiController]
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
        // [Authorize(Policy = "NoneOnly")]
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
        [Authorize(Roles = "UserOnly")]
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
        public async Task<IActionResult> Register([FromBody]RegisterUserCommand command)
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
        [Authorize(Policy = "UserOnly")]
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
        [Authorize(Roles = "UserOnly")]
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