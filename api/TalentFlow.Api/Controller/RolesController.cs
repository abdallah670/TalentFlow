using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentFlow.Application.Features.Roles.Command.AssignPermissions;
using TalentFlow.Application.Features.Roles.Command.CreateRole;
using TalentFlow.Application.Features.Roles.Query.GetRoles;

namespace TalentFlow.Api.Controller
{
    [Route("api/v1/roles")]
    [Authorize(Roles = "TenantAdmin")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IMediator mediator;

        public RolesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task <IActionResult> GetRoles()
        {
            var res = await mediator.Send(new GetRolesQuery());
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody]CreateRoleCommand command)
        {
            var res =await mediator.Send(command);
            if(!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);

        }

        [HttpPost("{id}/permissions")]
        public async Task<IActionResult> AssignPermissions(
      Guid id,
      [FromBody] AssignPermissionsCommand command)
        {
            command.RoleId = id;

            var result = await mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


    }

    
}
