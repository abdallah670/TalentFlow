using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Interfaces;

namespace TalentFlow.Application.Features.Authentication.Commands.Logout
{
    public class LogoutCommandHandler :IRequestHandler<LogoutCommand,bool>
    {
        private readonly IRefreshTokenService _refreshTokenService;
        public LogoutCommandHandler(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }
        public async Task<bool> Handle(
    LogoutCommand request,
    CancellationToken cancellationToken)
        {
            await _refreshTokenService
                .RevokeRefreshTokenAsync(request.RefreshToken);

            return true;
        }
    }
}
