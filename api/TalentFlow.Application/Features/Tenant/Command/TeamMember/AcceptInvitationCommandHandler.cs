using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Models.Identity;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Tenant.Command.TeamMember
{
    public class AcceptInvitationCommandHandler
        : IRequestHandler<AcceptInvitationCommand, AuthResponse>
    {
        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly JwtSettings _jwtSettings;

        public AcceptInvitationCommandHandler(
            UserManager<Domain.Entities.IdentityModule. User> userManager,
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

        public async Task<AuthResponse> Handle(
            AcceptInvitationCommand request,
            CancellationToken cancellationToken)
        {
            // ==========================================
            // 1. Find invitation
            // ==========================================

            var invitation =
                (await _unitOfWork.Invitations.FindAsync(
                    x => x.Token == request.Token))
                .FirstOrDefault();

            if (invitation == null)
            {
                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message = "Invalid invitation."
                };
            }

            // ==========================================
            // 2. Check expiration
            // ==========================================

            if (invitation.ExpirationDate < DateTime.UtcNow)
            {
                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message = "This invitation has expired."
                };
            }

            // ==========================================
            // 3. Check if already accepted
            // ==========================================

            if (invitation.IsAccepted)
            {
                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message =
                        "This invitation has already been accepted."
                };
            }

            // ==========================================
            // 4. Check password
            // ==========================================

            if (request.Password != request.ConfirmPassword)
            {
                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message = "Passwords do not match."
                };
            }

            // ==========================================
            // 5. Check existing user
            // ==========================================

            var existingUser =
                await _userManager.FindByEmailAsync(
                    invitation.Email);

            if (existingUser != null)
            {
                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message =
                        "This email already has an account."
                };
            }

            // ==========================================
            // 6. Create user
            // ==========================================

            var user = new Domain.Entities.IdentityModule.User
            {
                FirstName = invitation.FirstName,

                LastName = invitation.LastName,

                UserName = invitation.Email,

                Email = invitation.Email,

                TenantId = invitation.TenantId,

                EmailConfirmed = true,

                IsActive = true
            };

            var createResult =
                await _userManager.CreateAsync(
                    user,
                    request.Password);

            if (!createResult.Succeeded)
            {
                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message = string.Join(
                        ", ",
                        createResult.Errors
                            .Select(x => x.Description))
                };
            }

            // ==========================================
            // 7. Assign invitation role
            // ==========================================

            var roleResult =
                await _userManager.AddToRoleAsync(
                    user,
                    invitation.Role.ToString());

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message = string.Join(
                        ", ",
                        roleResult.Errors
                            .Select(x => x.Description))
                };
            }
            await _unitOfWork.UserTenants.AddAsync(new TalentFlow.Domain.Entities.TenantModule.UserTenant
            {
                UserId = user.Id,
                TenantId = invitation.TenantId,
                Role = invitation.Role.ToString()
            });

            // ==========================================
            // 8. Mark invitation as accepted
            // ==========================================

            invitation.IsAccepted = true;

            await _unitOfWork.Invitations
                .UpdateAsync(invitation);

            await _unitOfWork.CompleteAsync();

            // ==========================================
            // 9. Get roles
            // ==========================================

            var roles =
                await _userManager.GetRolesAsync(user);

            // ==========================================
            // 10. Create JWT
            // ==========================================

            var jwt =
                await _jwtService.CreateJwtToken(
                    user,
                    roles);

            var accessToken =
                new JwtSecurityTokenHandler()
                    .WriteToken(jwt);

            // ==========================================
            // 11. Create refresh token
            // ==========================================

            var refreshToken =
                await _refreshTokenService
                    .GenerateRefreshTokenAsync(user);

            // ==========================================
            // 12. Return authentication response
            // ==========================================

            return new AuthResponse
            {
                Id = user.Id.ToString(),

                UserName = user.UserName!,

                Email = user.Email!,

                IsAuthenticated = true,

                Roles = roles.ToList(),

                Token = accessToken,

                TokenExpiration = jwt.ValidTo,

                RefreshToken = refreshToken,

                RefreshTokenExpiration =
                    DateTime.UtcNow.AddDays(
                        _jwtSettings
                            .RefreshTokenDurationInDays)
            };
        }
    }
}