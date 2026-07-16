using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Authentication.Commands.changePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordComand, BaseCommandResponse>
    {
        private readonly UserManager<Domain.Entities.IdentityModule.User> userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRefreshTokenService refreshTokenService;
        public ChangePasswordCommandHandler(UserManager<Domain.Entities.IdentityModule.User> userManager, ICurrentUserService currentUserService, IRefreshTokenService refreshTokenService)
        {
            this.userManager = userManager;
            _currentUserService = currentUserService;
            this.refreshTokenService = refreshTokenService;
        }

        public async Task<BaseCommandResponse> Handle(ChangePasswordComand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(_currentUserService.UserId.ToString());
            if (user is null)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "User not found."
                };
            }

           var result= await userManager.ChangePasswordAsync(user, request.OLdPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(x => x.Description))
                };
            }
            return new BaseCommandResponse
            {
                Message = "Password changed successfully.",
                Success = true,

            };
        }
    }
}
