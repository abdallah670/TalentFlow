using MediatR;
using Microsoft.Extensions.Logging;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Authentication.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, BaseCommandResponse<bool>>
    {
        private readonly ILogger<LogoutCommandHandler> logger;
        private readonly IRefreshTokenService _refreshTokenService;
        public LogoutCommandHandler(IRefreshTokenService refreshTokenService, ILogger<LogoutCommandHandler> logger)
        {
            this.logger = logger;
            _refreshTokenService = refreshTokenService;
        }
        public async Task<BaseCommandResponse<bool>> Handle(
    LogoutCommand request,
    CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(LogoutCommandHandler));
            try
            {
                await _refreshTokenService
                    .RevokeRefreshTokenAsync(request.RefreshToken);

                return new BaseCommandResponse<bool>
                {
                    Success = true,
                    Message = "Logged out successfully.",
                    Data = true
                };
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Failed to revoke refresh token in {Handler}", nameof(LogoutCommandHandler));
                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = "Invalid refresh token.",
                    Data = false
                };
            }
        }
    }
}
