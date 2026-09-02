using MediatR;
using TalentFlow.Application.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Models.Identity;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Authontication.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler :IRequestHandler<RefreshTokenCommand, BaseCommandResponse<AuthResponse>>
    {
        private readonly ILogger<RefreshTokenCommandHandler> logger;
        private readonly IJWTService jWTService;
        private readonly UserManager<Domain.Entities.IdentityModule.User> userManager;
        private readonly IRefreshTokenService refreshTokenService;
        private readonly JwtSettings jwtSettings;

        public RefreshTokenCommandHandler(IJWTService jWTService, UserManager<Domain.Entities.IdentityModule.User> userManager, IRefreshTokenService refreshTokenService, IOptions<JwtSettings> jwtSettings, ILogger<RefreshTokenCommandHandler> logger)

        {
            this.logger = logger;
            this.jWTService = jWTService;
            this.userManager = userManager;
            this.refreshTokenService = refreshTokenService;
            this.jwtSettings = jwtSettings.Value;
        }

        public async Task<BaseCommandResponse<AuthResponse>> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(RefreshTokenCommandHandler));
            var user = await refreshTokenService.ValidateRefreshTokenAsync(request.RefreshToken);

            if (user is null)
            {
                return new BaseCommandResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Invalid Refresh Token"
                };
            }
            await refreshTokenService.RevokeRefreshTokenAsync(request.RefreshToken);

            var roles = await userManager.GetRolesAsync(user);
            var jwtToken = await jWTService.CreateJwtToken(user, roles);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken =await refreshTokenService.GenerateRefreshTokenAsync(user);

            return new BaseCommandResponse<AuthResponse>
            {
                Success = true,
                Data = new AuthResponse
                {
                Id = user.Id.ToString(),
                UserName = user.UserName!,
                Email = user.Email!,
                IsAuthenticated = true,
                Roles = roles.ToList(),

                Token = accessToken,
                TokenExpiration = jwtToken.ValidTo,

                RefreshToken = refreshToken,
                RefreshTokenExpiration =
        DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenDurationInDays)
                }
            };

        }

    }
}
