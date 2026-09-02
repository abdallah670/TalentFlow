using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Models;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Authentication.Commands.ForgetPassword
{
    public class ForgetPasswordHandler : IRequestHandler<ForgetPasswordComand, BaseCommandResponse<bool>>
    {
        private readonly ILogger<ForgetPasswordHandler> logger;
        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;
        private readonly IEmailService emailService;
        private readonly AppUrlSettings _appUrlSettings;

        public ForgetPasswordHandler(
            UserManager<Domain.Entities.IdentityModule.User> userManager,
            IEmailService emailService,
            IOptions<AppUrlSettings> appUrlSettings, ILogger<ForgetPasswordHandler> logger)
        {
            this.logger = logger;
            _userManager = userManager;
            this.emailService = emailService;
            _appUrlSettings = appUrlSettings.Value;
        }

        public async Task<BaseCommandResponse<bool>> Handle(ForgetPasswordComand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(ForgetPasswordHandler));
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new BaseCommandResponse<bool>
                {
                    Success = true,
                    Message = "If an account with this email exists, a reset link has been sent."
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);

            var link = $"{_appUrlSettings.ApiBaseUrl}/api/Auth/ResetPasswordForm?email={Uri.EscapeDataString(user.Email!)}&token={encodedToken}";

            await emailService.SendEmailAsync(
                user.Email!,
                "Reset Password",
                $"Click here to reset your password: {link}");

            return new BaseCommandResponse<bool>
            {
                Success = true,
                Message = "If an account with this email exists, a reset link has been sent."
            };
        }
    }
}