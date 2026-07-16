using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TalentFlow.Application.Features.Authentication.Commands.changePassword;
using TalentFlow.Application.Features.Authentication.Commands.ConfermEmail;
using TalentFlow.Application.Features.Authentication.Commands.ForgetPassword;
using TalentFlow.Application.Features.Authentication.Commands.Logout;
using TalentFlow.Application.Features.Authentication.Commands.ResetPasswors;
using TalentFlow.Application.Features.Authentication.Queries.GetProfile;
using TalentFlow.Application.Features.Authontication.Commands.Login;
using TalentFlow.Application.Features.Authontication.Commands.RefreshToken;
using TalentFlow.Application.Features.Authontication.Commands.Register;
using TalentFlow.Application.Features.Authontication.Commands.Update;


namespace TalentFlow.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await mediator.Send(command);
            return Ok(response);

        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await mediator.Send(command);
            return Ok(response);

        }
      

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {

            var response = await mediator.Send(command);
            return Ok(response);
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
        {

            var result = await mediator.Send(command);
            if (!result)
                return BadRequest("Invalid refresh token.");

            return Ok(new
            {
                Message = "Logged out successfully."
            });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordComand command)
        {
            var result = await mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await mediator.Send(new ConfirmEmailCommand { UserId = userId, Token = token });
            if (result.Success) return Ok("Email confirmed");
            return BadRequest("Invalid or expired confirmation link.");
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordComand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            var result = await mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("Profile")]
        public async Task<ActionResult<UserProfileDto>> GetProfile()
        {
            return Ok(await mediator.Send(new GetProfileQuery()));
        }
    }
}
