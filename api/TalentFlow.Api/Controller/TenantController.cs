using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TalentFlow.Application.Features.Tenant.Command.RegisterTenant;
using TalentFlow.Application.Features.Tenant.Command.TeamMember;
using TalentFlow.Application.Features.Tenant.Command.TeamMember.ResendInvitation;
using TalentFlow.Application.Features.Tenant.Command.UpdateTenantSettings;
using TalentFlow.Application.Features.Tenant.Queries.GetCurrentTenant;

namespace TalentFlow.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TenantController(IMediator mediator)
        {
            _mediator = mediator;
        }

       

        // ==========================================
        // Current Tenant
        // ==========================================

        [Authorize]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrent()
        {
            var result =
                await _mediator.Send(
                    new GetCurrentTenantQuery());

            return Ok(result);
        }

        // ==========================================
        // Update Tenant Settings
        // ==========================================

        [Authorize(Roles = "TenantAdmin")]
        [HttpPut("settings")]
        public async Task<IActionResult> UpdateSettings(
            [FromBody] UpdateTenantSettingsCommand command)
        {
            var result =
                await _mediator.Send(command);

            return Ok(result);
        }

        // ==========================================
        // Accept Invitation
        // ==========================================

        [AllowAnonymous]
        [HttpPost("accept-invitation")]
        public async Task<IActionResult> AcceptInvitation(
            [FromBody] AcceptInvitationCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result =
                await _mediator.Send(command);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            return Ok(result);
        }

        // ==========================================
        // Invite Team Member
        // ==========================================

        [Authorize(Roles = "TenantAdmin")]
        [HttpPost("invite-member")]
        public async Task<IActionResult> InviteMember(
            [FromBody] InviteTeamMemberCommand command)
        {
            // ==========================================
            // Get current user
            // ==========================================

            var userIdClaim =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(
                    userIdClaim,
                    out var userId))
            {
                return Unauthorized();
            }

            // ==========================================
            // Get current tenant
            // ==========================================

            var tenantIdClaim =
      User.FindFirst("TenantId")?.Value;

            if (!Guid.TryParse(
                    tenantIdClaim,
                    out var tenantId))
            {
                return Unauthorized();
            }

            // ==========================================
            // Override values from request
            // ==========================================

            command.InvitedByUserId = userId;

            command.TenantId = tenantId;

            // ==========================================
            // Send command
            // ==========================================

            var result =
                await _mediator.Send(command);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            return Ok(result);
        }
        [Authorize(Roles = "TenantAdmin")]
        [HttpPost("invite/resend")]
        public async Task<IActionResult> ResendInvite([FromBody] ResendInvitationCommand command)
        {
            var tenantIdClaim = User.FindFirst("TenantId")?.Value;

            if (!Guid.TryParse(tenantIdClaim, out var tenantId))
            {
                return Unauthorized();
            }

            command.TenantId = tenantId;

            var result = await _mediator.Send(command);

            if (!result.IsAuthenticated && result.Message != "Invitation resent successfully.")
                return BadRequest(result);

            return Ok(result);
        }
    }
}