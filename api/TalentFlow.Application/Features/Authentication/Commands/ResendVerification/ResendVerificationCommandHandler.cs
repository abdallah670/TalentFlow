using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Models;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Authentication.Commands.ResendVerification
{
    public class ResendVerificationCommandHandler : IRequestHandler<ResendVerificationCommand, BaseCommandResponse<bool>>
    {
        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;
        private readonly IEmailService _emailService;
        private readonly AppUrlSettings _appUrlSettings;

        public ResendVerificationCommandHandler(
            UserManager<Domain.Entities.IdentityModule.User> userManager,
            IEmailService emailService,
            IOptions<AppUrlSettings> appUrlSettings)
        {
            _userManager = userManager;
            _emailService = emailService;
            _appUrlSettings = appUrlSettings.Value;
        }

        public async Task<BaseCommandResponse<bool>> Handle(ResendVerificationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new BaseCommandResponse<bool> { Success = true, Message = "If this email is registered, a verification link has been sent." };
            }

            if (user.EmailConfirmed)
            {
                return new BaseCommandResponse<bool> { Success = true, Message = "This email is already verified." };
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);
            var confirmationLink = $"{_appUrlSettings.ApiBaseUrl}/api/Auth/ConfirmEmail?userId={user.Id}&token={encodedToken}";

            try
            {
                await _emailService.SendEmailAsync(
                    user.Email!,
                    "Confirm your email",
                    $"<h2>Hello {user.FirstName}</h2><a href='{confirmationLink}'>Click here to confirm your email</a>");
            }
            catch
            {
            }

            return new BaseCommandResponse<bool> { Success = true, Message = "If this email is registered, a verification link has been sent." };
        }
    }
}