using MediatR;
using System;
using TalentFlow.Application.Models;

namespace TalentFlow.Application.Features.Tenant.Command.TeamMember.ResendInvitation
{
    public class ResendInvitationCommand : IRequest<AuthResponse>
    {
        public Guid TenantId { get; set; }
        public string Email { get; set; } = default!;
    }
}