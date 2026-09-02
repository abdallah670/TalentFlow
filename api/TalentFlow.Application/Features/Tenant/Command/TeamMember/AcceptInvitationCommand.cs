using MediatR;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Tenant.Command.TeamMember
{
    public class AcceptInvitationCommand : IRequest<BaseCommandResponse<AuthResponse>>
    {
        public string Token { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;
    }
}