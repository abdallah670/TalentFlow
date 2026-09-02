using MediatR;
using Microsoft.Extensions.Options;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Models;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Tenant.Command.TeamMember.ResendInvitation
{
   

   
        public class ResendInvitationCommandHandler : IRequestHandler<ResendInvitationCommand, BaseCommandResponse<bool>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmailService _emailService;
            private readonly AppUrlSettings _appUrlSettings;

            public ResendInvitationCommandHandler(
                IUnitOfWork unitOfWork,
                IEmailService emailService,
                IOptions<AppUrlSettings> appUrlSettings)
            {
                _unitOfWork = unitOfWork;
                _emailService = emailService;
                _appUrlSettings = appUrlSettings.Value;
            }

            public async Task<BaseCommandResponse<bool>> Handle(ResendInvitationCommand request, CancellationToken cancellationToken)
            {
                var invitation = (await _unitOfWork.Invitations.FindAsync(x =>
                        x.Email == request.Email &&
                        x.TenantId == request.TenantId &&
                        !x.IsAccepted))
                    .OrderByDescending(x => x.ExpirationDate)
                    .FirstOrDefault();

                if (invitation is null)
                {
                    return new BaseCommandResponse<bool>
                    {
                        Success = false,
                        Message = "No pending invitation found for this email."
                    };
                }

                invitation.ExpirationDate = DateTime.UtcNow.AddDays(7);

                await _unitOfWork.Invitations.UpdateAsync(invitation);
                await _unitOfWork.CompleteAsync();

                var invitationLink = $"{_appUrlSettings.FrontendBaseUrl}/accept-invitation?token={invitation.Token}";

                try
                {
                    await _emailService.SendEmailAsync(
                        invitation.Email,
                        "You're invited to join TalentFlow",
                        $"""
                    <h2>Hello {invitation.FirstName}</h2>

                    <p>
                        This is a reminder — you have been invited to join your company
                        on TalentFlow as <b>{invitation.Role}</b>.
                    </p>

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
                }
                catch
                {
                }

                return new BaseCommandResponse<bool>
                {
                    Success = true,
                    Message = "Invitation resent successfully."
                };
            }
        }
    }

