using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentFlow.Application.Features.User.Command.CreateUser;
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
        public async Task<IActionResult> GetUsers(GetUsersQuery command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await mediator.Send(command);
            return Ok(res);

        }


    }
}
