using MediatR;

namespace TalentFlow.Application.Features.Tenant.Queries.GetInvitationInfo
{
    public class GetInvitationInfoQuery : IRequest<InvitationInfoResponse>
    {
        public string Token { get; set; } = string.Empty;
    }

    public class InvitationInfoResponse
    {
        public bool IsValid { get; set; }
        public string Status { get; set; } = string.Empty; 
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
    }
}