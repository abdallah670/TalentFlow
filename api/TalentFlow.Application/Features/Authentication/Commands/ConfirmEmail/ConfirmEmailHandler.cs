using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Authentication.Commands.ConfermEmail
{
    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, BaseCommandResponse>
    {
        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;
        private readonly IEmailService emailService;

        public ConfirmEmailHandler(UserManager<Domain.Entities.IdentityModule.User> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            this.emailService = emailService;
        }

        public async Task<BaseCommandResponse> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
                return new BaseCommandResponse
                {
                    Message = "User Not found",
                    Success = false
                };

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);

            if (!result.Succeeded)
                return new BaseCommandResponse
                {
                    Message = "Invalid or expired confirmation link.",
                    Success = false
                };

            return new BaseCommandResponse
            {
                Message = "Email confirmed successfully.",
                Success = true
            };
        }
    }
}
