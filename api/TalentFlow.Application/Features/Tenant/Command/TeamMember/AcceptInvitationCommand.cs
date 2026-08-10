using MediatR;
using TalentFlow.Application.Models.Identity;

namespace TalentFlow.Application.Features.Tenant.Command.TeamMember
{
    public class AcceptInvitationCommand : IRequest<AuthResponse>
    {
        public string Token { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;
    }
}