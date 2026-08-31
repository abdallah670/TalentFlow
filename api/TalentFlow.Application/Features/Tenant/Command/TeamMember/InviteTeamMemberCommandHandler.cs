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
        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly AppUrlSettings _appUrlSettings;

        public InviteTeamMemberCommandHandler(
            UserManager<Domain.Entities.IdentityModule.User> userManager,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            IOptions<AppUrlSettings> appUrlSettings)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _appUrlSettings = appUrlSettings.Value;
        }

        public async Task<AuthResponse> Handle(
            InviteTeamMemberCommand request,
            CancellationToken cancellationToken)
        {
            // ==========================================
            // 1. Check existing invitation
            // ==========================================

            var existingInvitation =
                await _unitOfWork.Invitations.FindAsync(x =>
                    x.Email == request.Email &&
                    x.TenantId == request.TenantId &&
                    !x.IsAccepted &&
                    x.ExpirationDate > DateTime.UtcNow);

            if (existingInvitation.Any())
            {
                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message =
                        "An invitation has already been sent to this email."
                };
            }

            // ==========================================
            // 2. Check existing user
            // ==========================================

            var existingUser =
                await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null &&
                existingUser.TenantId == request.TenantId)
            {
                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message =
                        "This user is already a member of your company."
                };
            }

            // ==========================================
            // 3. Create invitation token
            // ==========================================

            var token = Guid.NewGuid().ToString("N");

            // ==========================================
            // 4. Create invitation
            // ==========================================

            var invitation = new Invitation
            {
                TenantId = request.TenantId,

                InvitedByUserId = request.InvitedByUserId,

                FirstName = request.FirstName,

                LastName = request.LastName,

                Email = request.Email,

                Role = request.Role,

                Token = token,

                ExpirationDate =
                    DateTime.UtcNow.AddDays(7),

                CustomMessage =
                    request.CustomMessage,

                IsAccepted = false
            };

            await _unitOfWork.Invitations.AddAsync(invitation);

            await _unitOfWork.CompleteAsync();

            // ==========================================
            // 5. Create invitation link
            // ==========================================

            var invitationLink =
                $"{_appUrlSettings.ApiBaseUrl}" +
                $"/api/Tenant/accept-invitation?token={token}";

            // ==========================================
            // 6. Send Email
            // ==========================================

            await _emailService.SendEmailAsync(
                request.Email,
                "You're invited to join TalentFlow",
                $"""
                <h2>Hello {request.FirstName}</h2>

                <p>
                    You have been invited to join your company
                    on TalentFlow as
                    <b>{request.Role}</b>.
                </p>

                {(string.IsNullOrWhiteSpace(request.CustomMessage)
                    ? ""
                    : $"<p>{request.CustomMessage}</p>")}

                <p>
                    Click the link below to accept the invitation:
                </p>

                <a href="{invitationLink}">
                    Accept Invitation
                </a>

                <p>
                    This invitation expires in 7 days.
                </p>
                """);

            // ==========================================
            // 7. Response
            // ==========================================

            return new AuthResponse
            {
                IsAuthenticated = false,
                Message = "Invitation sent successfully."
            };
        }
    }
}