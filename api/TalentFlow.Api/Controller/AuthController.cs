using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TalentFlow.Application.Features.Authentication.Commands.changePassword;
using TalentFlow.Application.Features.Authentication.Commands.ConfermEmail;
using TalentFlow.Application.Features.Authentication.Commands.ForgetPassword;
using TalentFlow.Application.Features.Authentication.Commands.Logout;
using TalentFlow.Application.Features.Authentication.Commands.ResendVerification;
using TalentFlow.Application.Features.Authentication.Commands.ResetPasswors;
using TalentFlow.Application.Features.Authentication.Commands.VerifyEmail;
using TalentFlow.Application.Features.Authentication.Queries.EmailStatus;
using TalentFlow.Application.Features.Authentication.Queries.GetProfile;
using TalentFlow.Application.Features.Authentication.Queries.GetUserTenants;
using TalentFlow.Application.Features.Authontication.Commands.Login;
using TalentFlow.Application.Features.Authontication.Commands.RefreshToken;
using TalentFlow.Application.Features.Authontication.Commands.Register;
using TalentFlow.Application.Features.Authontication.Commands.SelectTenant;
using TalentFlow.Application.Features.Authontication.Commands.Update;
using TalentFlow.Application.Features.Tenant.Queries.GetInvitationInfo;


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
        [HttpPost("register-employer")]
        public async Task<IActionResult> RegisterEmployer(
    [FromBody] TenantRegisterCommand command)
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
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
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
        public async Task<IActionResult> GetProfile()
        {
            var result = await mediator.Send(new GetProfileQuery());

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("ResetPasswordForm")]
        public IActionResult ResetPasswordForm([FromQuery] string email, [FromQuery] string token)
        {
            var html = $@"
<!DOCTYPE html>
<html>
<head><title>Reset Password</title></head>
<body>
    <h2>Reset Your Password</h2>
    <form id='resetForm'>
        <input type='hidden' id='email' value='{System.Net.WebUtility.HtmlEncode(email)}' />
        <input type='hidden' id='token' value='{System.Net.WebUtility.HtmlEncode(token)}' />
        <label>New Password:</label><br/>
        <input type='password' id='newPassword' required /><br/><br/>
        <label>Confirm Password:</label><br/>
        <input type='password' id='confirmPassword' required /><br/><br/>
        <button type='submit'>Reset Password</button>
    </form>
    <p id='result'></p>

    <script>
        document.getElementById('resetForm').addEventListener('submit', async function(e) {{
            e.preventDefault();
            const response = await fetch('/api/Auth/reset-password', {{
                method: 'POST',
                headers: {{ 'Content-Type': 'application/json' }},
                body: JSON.stringify({{
                    email: document.getElementById('email').value,
                    token: document.getElementById('token').value,
                    newPassword: document.getElementById('newPassword').value,
                    confirmPassword: document.getElementById('confirmPassword').value
                }})
            }});
            const data = await response.json();
            document.getElementById('result').innerText = data.message;
        }});
    </script>
</body>
</html>";

            return Content(html, "text/html");
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAlias([FromBody] RefreshTokenCommand command)
        {
            var response = await mediator.Send(command);
            return Ok(response);
        }
        [Authorize]
        [HttpPost("select-tenant")]
        public async Task<IActionResult> SelectTenant([FromBody] SelectTenantCommand command)
        {
            var purpose = User.FindFirst("purpose")?.Value;
            if (purpose != "tenant_selection")
                return Unauthorized("Invalid selection token.");

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            command.UserId = userId;

            var response = await mediator.Send(command);
            return Ok(response);
        }
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand command)
        {
            var result = await mediator.Send(command);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("resend-verification")]
        public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("email-status")]
        public async Task<IActionResult> EmailStatus([FromBody] EmailStatusQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("tenants")]
        public async Task<IActionResult> GetTenants()
        {
            var userIdClaim = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await mediator.Send(new GetUserTenantsQuery { UserId = userId });
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("invitation-info")]
        public async Task<IActionResult> GetInvitationInfo([FromQuery] string token)
        {
            var result = await mediator.Send(new GetInvitationInfoQuery { Token = token });
            return Ok(result);
        }
    }
}
