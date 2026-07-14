using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Models.Identity;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Authontication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IJWTService jWTService;
        private readonly UserManager<Domain.Entities.IdentityModule.User> userManager;
        private readonly IRefreshTokenService refreshTokenService;
        private readonly JwtSettings jwtSettings;

        public LoginCommandHandler(IJWTService jWTService, UserManager<Domain.Entities.IdentityModule.User> userManager, IRefreshTokenService refreshTokenService, IOptions<JwtSettings> jwtSettings)

        {
            this.jWTService = jWTService;
            this.userManager = userManager;
            this.refreshTokenService = refreshTokenService;
            this.jwtSettings = jwtSettings.Value;
        }
        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message = "Invalid Email or Password"
                };
            }
            var isPasswordCorrect = await userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordCorrect)
            {
                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message = "Invalid Email or Password"
                };
            }

            if (!user.IsActive)
            {
                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message = "Your account is inactive"
                };
            }
            var roles = await userManager.GetRolesAsync(user);

            var jwtToken = await jWTService.CreateJwtToken(user, roles);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var refreshToken = await refreshTokenService.GenerateRefreshTokenAsync(user);
            return new AuthResponse
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
            };
        }

    }
}
