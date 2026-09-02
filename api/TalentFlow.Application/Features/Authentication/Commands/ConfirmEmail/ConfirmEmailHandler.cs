using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Authentication.Commands.ConfermEmail
{
    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, BaseCommandResponse<bool>>
    {
        private readonly ILogger<ConfirmEmailHandler> logger;
        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;
        private readonly IEmailService emailService;

        public ConfirmEmailHandler(UserManager<Domain.Entities.IdentityModule.User> userManager, IEmailService emailService, ILogger<ConfirmEmailHandler> logger)
        {
            this.logger = logger;
            _userManager = userManager;
            this.emailService = emailService;
        }

        public async Task<BaseCommandResponse<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(ConfirmEmailHandler));
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
                return new BaseCommandResponse<bool>
                {
                    Message = "User Not found",
                    Success = false
                };

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);

            if (!result.Succeeded)
                return new BaseCommandResponse<bool>
                {
                    Message = "Invalid or expired confirmation link.",
                    Success = false
                };

            return new BaseCommandResponse<bool>
            {
                Message = "Email confirmed successfully.",
                Success = true
            };
        }
    }
}
