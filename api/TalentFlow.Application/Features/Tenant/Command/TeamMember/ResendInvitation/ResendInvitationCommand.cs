using MediatR;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Tenant.Command.TeamMember.ResendInvitation
{
    public class ResendInvitationCommand : IRequest<BaseCommandResponse<bool>>
    {
        public Guid TenantId { get; set; }
        public string Email { get; set; } = default!;
    }
}