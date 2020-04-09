using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Interfaces.Services;
using Identity.Handlers;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Policy = "Default"), Route("api/users"), ApiVersion("1.0"), ApiController]
    public class UsersController : ControllerBase
    {
        private UserHandler _handler;

        public UsersController(IUserService userService, UserHandler handler) => _handler = handler;




        // GET: api/users/authenticate
        [AllowAnonymous]
        [HttpGet, Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromForm]AuthenticateUserCommand command)
        {
            var result = await _handler.Handler(command);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }



        // GET: api/users/forgot-password
        [AllowAnonymous]
        [HttpGet, Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromForm]ForgotPasswordCommand command)
        {
            var result = await _handler.Handler(command);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }



        // PATCH: api/users/reset-password/email=exemplo@exemplo.com&token=CfDJ8DW0ycF3tRJEv5qbV+goZqrnl8lIKxhbWixoWigvGFLxLrSZQw6Lga
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



        // PATCH: api/users/change-password
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



        // POST: api/users
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm]RegisterUserCommand command)
        {
            var result = await _handler.Handler(command);
            if (result.Succeeded)
                return Created($"api/users/profile", result);
            return BadRequest(result);
        }



        // GET: api/users/profile
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



        // PATCH: api/users/change-name
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