using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Models;
using TalentFlow.Application.Models.Identity;
using TalentFlow.Domain.Entities.IdentityModule;
using TalentFlow.Domain.Enums;

namespace TalentFlow.Application.Features.Authontication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly UserManager<Domain.Entities.IdentityModule.User> userManager;
        private readonly IJWTService jWTService;
        private readonly IRefreshTokenService refreshTokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly IEmailService emailService;
        private readonly AppUrlSettings _appUrlSettings;

        public RegisterCommandHandler(
            UserManager<Domain.Entities.IdentityModule.User> userManager,
            IJWTService jWTService,
            IRefreshTokenService refreshTokenService,
            IOptions<JwtSettings> jwtSettings,
            IEmailService emailService,
            IOptions<AppUrlSettings> appUrlSettings) // ✅ IOptions مش الكلاس مباشرة
        {
            this.userManager = userManager;
            this.jWTService = jWTService;
            this.refreshTokenService = refreshTokenService;
            _jwtSettings = jwtSettings.Value;
            this.emailService = emailService;
            _appUrlSettings = appUrlSettings.Value; // ✅ .Value
        }

        public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existinguser = await userManager.FindByEmailAsync(request.Email);
            if (existinguser is not null)
            {
                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message = "Email Already Exist"
                };
            }

            var user = new Domain.Entities.IdentityModule.User
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                IsActive = true
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message = string.Join(", ", result.Errors.Select(x => x.Description))
                };
            }

            await userManager.AddToRoleAsync(user, Roles.Candidate.ToString());
            var roles = await userManager.GetRolesAsync(user);
            var JwtToken = await jWTService.CreateJwtToken(user, roles);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(JwtToken);
            var refreshToken = await refreshTokenService.GenerateRefreshTokenAsync(user);

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);

            var confirmationLink = $"{_appUrlSettings.ApiBaseUrl}/api/Auth/ConfirmEmail?userId={user.Id}&token={encodedToken}";
            Console.WriteLine("LINK: " + confirmationLink);
            var body = $"<h2>Welcome {user.FirstName}</h2><a href='{confirmationLink}'>Click here to confirm your email</a>";

            await emailService.SendEmailAsync(user.Email, "Confirm Email", body);

            return new AuthResponse
            {
                Id = user.Id.ToString(),
                UserName = user.UserName!,
                Email = user.Email!,
                IsAuthenticated = true,
                Roles = roles.ToList(),
                Message = "Registered successfully. Please check your email to confirm your account.",
                Token = accessToken,
                TokenExpiration = JwtToken.ValidTo,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationInDays)
            };
        }
    }
}