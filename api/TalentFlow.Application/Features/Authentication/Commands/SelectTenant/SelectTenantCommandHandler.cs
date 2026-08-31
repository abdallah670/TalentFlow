using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Models.Identity;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Authontication.Commands.SelectTenant
{
    public class SelectTenantCommandHandler : IRequestHandler<SelectTenantCommand, AuthResponse>
    {
        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly JwtSettings _jwtSettings;

        public SelectTenantCommandHandler(
            UserManager<Domain.Entities.IdentityModule.User> userManager,
            IUnitOfWork unitOfWork,
            IJWTService jwtService,
            IRefreshTokenService refreshTokenService,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResponse> Handle(SelectTenantCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                return new AuthResponse { IsAuthenticated = false, Message = "User not found." };

            var memberships = await _unitOfWork.UserTenants.FindAsync(
                x => x.UserId == request.UserId && x.TenantId == request.TenantId && x.IsActive);

            if (!memberships.Any())
                return new AuthResponse { IsAuthenticated = false, Message = "You are not a member of this workspace." };

            var roles = await _userManager.GetRolesAsync(user);

            var jwtToken = await _jwtService.CreateJwtToken(user, roles, request.TenantId);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user);

            return new AuthResponse
            {
                Id = user.Id.ToString(),
                UserName = user.UserName!,
                Email = user.Email!,
                Roles = roles.ToList(),
                IsAuthenticated = true,
                Token = accessToken,
                TokenExpiration = jwtToken.ValidTo,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationInDays)
            };
        }
    }
}