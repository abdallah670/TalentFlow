using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentFlow.Application.Features.Authontication.Commands.Update;
using TalentFlow.Application.Features.User.Command.CreateUser;
using TalentFlow.Application.Features.User.Command.DisableUser;
using TalentFlow.Application.Features.User.Query.GetUser;

namespace TalentFlow.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [Authorize(Roles = "TenantAdmin")]
        [HttpPost("Create_user")]
        public async Task<IActionResult> CreateUser(CreateUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await mediator.Send(command);
            return Ok(res);

        }

        [Authorize(Roles = "TenantAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await mediator.Send(command);
            return Ok(res);

        }

        [Authorize(Roles = "TenantAdmin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await mediator.Send(new GetUserQuery { Id = id });

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [Authorize(Roles = "TenantAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id,UpdateUserCommand command)
        {
            var result = await mediator.Send(command);

            

            return Ok(result);
        }

        [Authorize(Roles = "TenantAdmin")]
        [HttpPatch("{id}/disable")]
        public async Task<IActionResult> DisAbleUser(Guid id)
        {
            var result = await mediator.Send(new DisAbleUserCommand
            {
                Id = id
            });

            return Ok(result);
        }


    }
}
