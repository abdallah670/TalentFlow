using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Authentication.Commands.changePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordComand, BaseCommandResponse<bool>>
    {
        private readonly ILogger<ChangePasswordCommandHandler> logger;
        private readonly UserManager<Domain.Entities.IdentityModule.User> userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRefreshTokenService refreshTokenService;
        public ChangePasswordCommandHandler(UserManager<Domain.Entities.IdentityModule.User> userManager, ICurrentUserService currentUserService, IRefreshTokenService refreshTokenService, ILogger<ChangePasswordCommandHandler> logger)
        {
            this.logger = logger;
            this.userManager = userManager;
            _currentUserService = currentUserService;
            this.refreshTokenService = refreshTokenService;
        }

        public async Task<BaseCommandResponse<bool>> Handle(ChangePasswordComand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(ChangePasswordCommandHandler));
            var user = await userManager.FindByIdAsync(_currentUserService.UserId.ToString());
            if (user is null)
            {
                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = "User not found."
                };
            }

           var result= await userManager.ChangePasswordAsync(user, request.OLdPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(x => x.Description))
                };
            }
            return new BaseCommandResponse<bool>
            {
                Message = "Password changed successfully.",
                Success = true,

            };
        }
    }
}
