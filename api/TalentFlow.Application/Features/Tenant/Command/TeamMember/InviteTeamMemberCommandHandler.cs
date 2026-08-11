using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Models;
using TalentFlow.Application.Models.Identity;
using TalentFlow.Domain.Entities.CandidateModule;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Tenant.Command.TeamMember
{
    public class InviteTeamMemberCommandHandler
        : IRequestHandler<InviteTeamMemberCommand, AuthResponse>
    {
        private readonly UserManager<Domain.Entities.IdentityModule.User> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailService emailService;
        private readonly AppUrlSettings appUrlSettings;

        public InviteTeamMemberCommandHandler(
            UserManager<Domain.Entities.IdentityModule.User> userManager,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            IOptions<AppUrlSettings> appUrlSettings)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
            this.appUrlSettings = appUrlSettings.Value;
        }

        public async Task<AuthResponse> Handle(
            InviteTeamMemberCommand request,
            CancellationToken cancellationToken)
        {
            var existingInvitation = await unitOfWork.Invitations
    .FindAsync(x =>
      x.Email == request.Email &&
x.TenantId == request.TenantId &&
!x.IsAccepted &&
x.ExpirationDate > DateTime.UtcNow);

            if (existingInvitation.Any())
            {
                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message = "An invitation has already been sent to this email."
                };
            }
            var existingUser = await userManager.FindByEmailAsync(request.Email);

            if (existingUser != null && existingUser.TenantId == request.TenantId)
            {
                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message = "This user is already a member of your company."
                };
            }
            var token = Guid.NewGuid().ToString("N");
            var invitation = new Invitation
            {
                TenantId = request.TenantId,
                InvitedByUserId = request.InvitedByUserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Role = request.Role,
                Token = token,
                ExpirationDate = DateTime.UtcNow.AddDays(7),
                CustomMessage = request.CustomMessage
            };

            await unitOfWork.Invitations.AddAsync(invitation);
            await unitOfWork.CompleteAsync();
            var invitationLink =
    $"{appUrlSettings.ApiBaseUrl}/api/Auth/AcceptInvitation?token={token}";
            await emailService.SendEmailAsync(
    request.Email,
    "You're invited to join TalentFlow",
    $@"
    <h2>Hello {request.FirstName}</h2>

    <p>You have been invited to join your company on TalentFlow as <b>{request.Role}</b>.</p>

    <p>{request.CustomMessage}</p>

    <a href='{invitationLink}'>Accept Invitation</a>

    <p>This invitation expires in 7 days.</p>
    ");
            return new AuthResponse
            {
                IsAuthenticated = true,
                Message = "Invitation sent successfully."
            };
        }
    }
}