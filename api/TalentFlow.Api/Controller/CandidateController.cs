using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TalentFlow.Application.Features.CandidateModule.Commands.UpdatePreferences;
using TalentFlow.Application.Features.CandidateModule.Commands.UpdateProfessionalProfile;
using TalentFlow.Application.Features.CandidateModule.Commands.UpdateSkills;
using TalentFlow.Application.Features.CandidateModule.Commands.UploadResume;
using TalentFlow.Application.Features.CandidateModule.Queries.GetMyProfile;

namespace TalentFlow.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/candidate")]
    public class CandidateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CandidateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private Guid CurrentUserId =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var result = await _mediator.Send(new GetMyProfileQuery { UserId = CurrentUserId });
            return Ok(result);
        }

        [HttpPatch("professional-profile")]
        public async Task<IActionResult> UpdateProfessionalProfile([FromBody] UpdateProfessionalProfileCommand command)
        {
            command.UserId = CurrentUserId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("resume")]
        public async Task<IActionResult> UploadResume([FromForm] UploadResumeCommand command)
        {
            command.UserId = CurrentUserId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("preferences")]
        public async Task<IActionResult> UpdatePreferences([FromBody] UpdatePreferencesCommand command)
        {
            command.UserId = CurrentUserId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPatch("skills")]
        public async Task<IActionResult> UpdateSkills([FromBody] UpdateSkillsCommand command)
        {
            command.UserId = CurrentUserId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}