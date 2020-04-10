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
    [Authorize(Policy = "Default"), Route("{culture:culture}/users"), ApiVersion("1.0"), ApiController]
    public class UsersController : ControllerBase
    {
        private UserHandler _handler;
        private IStringLocalizer<UsersController> _localizer;

        public UsersController(IStringLocalizer<UsersController> localizer, UserHandler handler)
        {
            _handler = handler;
            _localizer = localizer;
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            // var culture = Request.Path.Value.Split('/')[3]?.ToString();
            // var command = new CommandResult(true, _localizer.GetString("USER_FOUND").Value, _localizer.GetString("USER_FOUND"));
            return Ok(_localizer.GetString("USER_FOUND").Value);
        }


        // GET: https://api.localhost:5001/en-us/users/authenticate
        [AllowAnonymous]
        [HttpGet, Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromForm]AuthenticateUserCommand command)
        {
            var result = await _handler.Handler(command);
            if (result.Succeeded)
            {
                result.Message = _localizer.GetString("USER_AUTHENTICATE_SUCCESS");
                return Ok(result);
            }
            return BadRequest(result);
        }



        // GET: https://api.localhost:5001/en-us/users/forgot-password
        [AllowAnonymous]
        [HttpGet, Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromForm]ForgotPasswordCommand command)
        {
            var result = await _handler.Handler(command);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }



        // PATCH: https://api.localhost:5001/en-us/users/reset-password/email=exemplo@exemplo.com&token=CfDJ8DW0ycF3tRJEv5qbVtaA2vc4
        [AllowAnonymous]
        [HttpPatch, Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery]string email, [FromQuery]string token, [FromForm]ResetPasswordCommand command)
        {
            command.Email = email;
            command.Token = token;
            var result = await _handler.Handler(command);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }



        // PATCH: https://api.localhost:5001/en-us/users/change-password
        [Authorize(Roles = "User")]
        [HttpPatch, Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromForm]ChangePasswordCommand command)
        {
            command.Email = Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var result = await _handler.Handler(command);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }



        // POST: https://api.localhost:5001/en-us/users
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm]RegisterUserCommand command)
        {
            var result = await _handler.Handler(command);
            if (result.Succeeded)
                return Created($"https://api.localhost:5001/en-us/users/profile", result);
            return BadRequest(result);
        }



        // GET: https://api.localhost:5001/en-us/users/profile
        [Authorize(Roles = "User")]
        [HttpGet, Route("profile")]
        public async Task<IActionResult> Profile([FromRoute]ProfileUserCommand command)
        {
            command.Email = Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var result = await _handler.Handler(command);
            if (result.Succeeded)
                return Ok(result);
            return NoContent();
        }



        // PATCH: https://api.localhost:5001/en-us/users/change-name
        [Authorize(Roles = "User")]
        [HttpPatch, Route("change-name")]
        public async Task<IActionResult> ChangeName([FromForm]ChangeNameCommand command)
        {
            command.Email = Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var result = await _handler.Handler(command);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest();
        }
    }
}