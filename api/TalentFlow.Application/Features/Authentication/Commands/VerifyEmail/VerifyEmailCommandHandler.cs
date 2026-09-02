using MediatR;
using Microsoft.AspNetCore.Identity;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Authentication.Commands.VerifyEmail
{
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, BaseCommandResponse<bool>>
    {
        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;

        public VerifyEmailCommandHandler(UserManager<Domain.Entities.IdentityModule.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<BaseCommandResponse<bool>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new BaseCommandResponse<bool> { Success = false, Message = "Invalid or expired confirmation link." };
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (!result.Succeeded)
            {
                return new BaseCommandResponse<bool> { Success = false, Message = "Invalid or expired confirmation link." };
            }

            if (!user.IsActive)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
            }

            return new BaseCommandResponse<bool> { Success = true, Message = "Email confirmed successfully." };
        }
    }
}