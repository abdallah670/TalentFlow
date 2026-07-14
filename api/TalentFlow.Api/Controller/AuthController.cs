using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


        


        [Authorize]
        [HttpGet("Profile")]
        public async Task<ActionResult<UserProfileDto>> GetProfile()
        {
            return Ok(await mediator.Send(new GetProfileQuery()));
        }
    }
}
