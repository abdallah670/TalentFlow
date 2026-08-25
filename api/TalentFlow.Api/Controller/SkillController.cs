using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentFlow.Application.Features.Skills.Command.CreateSkillCommand;
using TalentFlow.Application.Features.Skills.Command.UpdateSkillCommand;
using TalentFlow.Application.Features.Skills.Queries.GetskillsQuery;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Api.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class SkillController : ControllerBase
    {
        private readonly IMediator mediator;

        public SkillController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetSkills([FromQuery] string? search)
        {
            var res = await mediator.Send(new GetSkillsQuery { Search = search });
            return Ok(res);
        }
        [HttpPost]
        [Authorize(Roles = nameof(Domain.Enums.Roles.SystemAdmin))]
        public async Task<IActionResult> CreateSkill([FromBody] CreateSkillCommand command)
        {
            var res = await mediator.Send(command);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Domain.Enums.Roles.SystemAdmin))]
        public async Task<IActionResult> UpdateSkill(Guid id,[FromBody] UpdateSkillCommand command)
        {
            command.id = id;
            var res = await mediator.Send(command);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }
    }
}
