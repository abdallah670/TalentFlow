using MediatR;
using TalentFlow.Application.Contracts.Persistence;

namespace TalentFlow.Application.Features.Tenant.Queries.GetInvitationInfo
{
    public class GetInvitationInfoQueryHandler : IRequestHandler<GetInvitationInfoQuery, InvitationInfoResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetInvitationInfoQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InvitationInfoResponse> Handle(GetInvitationInfoQuery request, CancellationToken cancellationToken)
        {
            var invitation = (await _unitOfWork.Invitations.FindAsync(x => x.Token == request.Token))
                .FirstOrDefault();

            if (invitation is null)
            {
                return new InvitationInfoResponse { IsValid = false, Status = "invalid" };
            }

            if (invitation.IsAccepted)
            {
                return new InvitationInfoResponse { IsValid = false, Status = "used" };
            }

            if (invitation.ExpirationDate < DateTime.UtcNow)
            {
                return new InvitationInfoResponse { IsValid = false, Status = "expired" };
            }

            var tenant = await _unitOfWork.Tenants.GetByIdAsync(invitation.TenantId);

            return new InvitationInfoResponse
            {
                IsValid = true,
                Status = "valid",
                FirstName = invitation.FirstName,
                LastName = invitation.LastName,
                Email = invitation.Email,
                Role = invitation.Role.ToString(),
                CompanyName = tenant?.Name ?? ""
            };
        }
    }
}