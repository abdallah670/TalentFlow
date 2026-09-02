
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TalentFlow.Application.Features.Authentication.Commands.ResetPasswors;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.IdentityModule;

namespace ChefNear.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, BaseCommandResponse<bool>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ILogger<ResetPasswordCommandHandler> _logger;

        public ResetPasswordCommandHandler(
            UserManager<User> userManager,
            IRefreshTokenService refreshTokenService,
            ILogger<ResetPasswordCommandHandler> logger)
        {
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
            _logger = logger;
        }

        public async Task<BaseCommandResponse<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning($"Reset password failed: User not found with email {request.Email}");
                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = "Passwords do not match."
                };
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning($"Reset password failed for user {request.Email}: {errors}");
                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = errors
                };
            }

            try
            {

                 await _refreshTokenService.RevokeRefreshTokenAsync(user.Id.ToString());

                _logger.LogInformation($"Refresh tokens revoked for user {user.Email}");
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning(ex, $"Failed to revoke refresh tokens for user {user.Email}");
            }

            _logger.LogInformation($"Password reset successfully for user {request.Email}");

            return new BaseCommandResponse<bool>
            {
                Success = true,
                Message = "Password reset successfully."
            };
        }
    }
}