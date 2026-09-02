using MediatR;
using TalentFlow.Application.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Models.Identity;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Authontication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, BaseCommandResponse<AuthResponse>>
    {
        private readonly ILogger<LoginCommandHandler> logger;
        private readonly IJWTService jWTService;
        private readonly UserManager<Domain.Entities.IdentityModule.User> userManager;
        private readonly IRefreshTokenService refreshTokenService;
        private readonly JwtSettings jwtSettings;
        private readonly IcandidateProfileRepo candidateProfileRepo;
        public LoginCommandHandler(IJWTService jWTService, UserManager<Domain.Entities.IdentityModule.User> userManager, IRefreshTokenService refreshTokenService, IOptions<JwtSettings> jwtSettings, IcandidateProfileRepo candidateProfileRepo, ILogger<LoginCommandHandler> logger)

        {
            this.logger = logger;
            this.jWTService = jWTService;
            this.userManager = userManager;
            this.refreshTokenService = refreshTokenService;
            this.jwtSettings = jwtSettings.Value;
            this.candidateProfileRepo = candidateProfileRepo;
        }
        public async Task<BaseCommandResponse<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Handling {Handler}", nameof(LoginCommandHandler));
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new BaseCommandResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Invalid Email or Password"
                };
            }

            if (await userManager.IsLockedOutAsync(user))
            {
                var lockoutEnd = await userManager.GetLockoutEndDateAsync(user);

                var minutes = Math.Ceiling(
                    (lockoutEnd.Value.UtcDateTime - DateTime.UtcNow).TotalMinutes);

                return new BaseCommandResponse<AuthResponse>
                {
                    Success = false,
                    Message = $"Too many failed attempts. Try again after {minutes} minute(s)."
                };
            }
            var result = await userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
            {
                await userManager.AccessFailedAsync(user);

                return new BaseCommandResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            await userManager.ResetAccessFailedCountAsync(user);
            if (!user.IsActive)
            {
                return new BaseCommandResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Your account is inactive"
                };
            }
            if (!user.EmailConfirmed)
            {
                return new BaseCommandResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Your Must Conferm Email"
                };
            }
            var roles = await userManager.GetRolesAsync(user);

            var jwtToken = await jWTService.CreateJwtToken(user, roles);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var refreshToken = await refreshTokenService.GenerateRefreshTokenAsync(user);
            var profile = await candidateProfileRepo
    .FindAsync(x => x.UserId == user.Id);

            var candidateProfile = profile.FirstOrDefault();
            int currentStep = 2;

            if (candidateProfile != null)
            {
                bool professional =
                    !string.IsNullOrWhiteSpace(candidateProfile.PhoneNumber);

                bool resume =
                    !string.IsNullOrWhiteSpace(candidateProfile.ResumeUrl);

                bool skills =
                    candidateProfile.Skills.Any();

                bool preferences =
                    !string.IsNullOrWhiteSpace(candidateProfile.PreferredLocation);

                if (!professional)
                    currentStep = 2;
                else if (!resume || !skills)
                    currentStep = 3;
                else if (!preferences)
                    currentStep = 4;
                else
                    currentStep = 5;
            }
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
         DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenDurationInDays),

                CurrentStep = currentStep,
                OnboardingCompleted = currentStep == 5
                }
            };
        }

    }
}
