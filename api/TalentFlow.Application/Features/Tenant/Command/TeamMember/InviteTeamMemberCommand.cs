using MediatR;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Enums;

namespace TalentFlow.Application.Features.Tenant.Command.TeamMember
{
    public class InviteTeamMemberCommand : IRequest<BaseCommandResponse<AuthResponse>>
    {
        public Guid TenantId { get; set; }

        public Guid InvitedByUserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public Domain.Enums.Roles Role { get; set; }

        public string? CustomMessage { get; set; }
    }
}