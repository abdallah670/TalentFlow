using MediatR;
using TalentFlow.Application.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Features.Tenant.Command.TeamMember;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Models.Identity;
using TalentFlow.Domain.Entities.IdentityModule;

public class AcceptInvitationCommandHandler
    : IRequestHandler<AcceptInvitationCommand, BaseCommandResponse<AuthResponse>>
{
        private readonly ILogger<AcceptInvitationCommandHandler> logger;
    private readonly UserManager<User> userManager;
    private readonly IUnitOfWork unitOfWork;
    private readonly IJWTService jwtService;
    private readonly IRefreshTokenService refreshTokenService;
    private readonly JwtSettings jwtSettings;

    public AcceptInvitationCommandHandler(UserManager<User> userManager, IUnitOfWork unitOfWork, IJWTService jwtService, IRefreshTokenService refreshTokenService, IOptions<JwtSettings> jwtSettings, ILogger<AcceptInvitationCommandHandler> logger)  

    {
            this.logger = logger;
        this.userManager = userManager;
        this.unitOfWork = unitOfWork;
        this.jwtService = jwtService;
        this.refreshTokenService = refreshTokenService;
        this.jwtSettings = jwtSettings.Value;  
    }

    public async Task<BaseCommandResponse<AuthResponse>> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Handler}", nameof(AcceptInvitationCommandHandler));
        var invitation = (await unitOfWork.Invitations.FindAsync(x => x.Token == request.Token)).FirstOrDefault();
        if (invitation is null)
        {
            return new BaseCommandResponse<AuthResponse>
            {
                Success = false,
                Message = "Invalid invitation."
            };
        }

        if (invitation.ExpirationDate < DateTime.UtcNow)
        {
            return new BaseCommandResponse<AuthResponse>
            {
                Success = false,
                Message = "This invitation has expired."
            };
        }

        if (invitation.IsAccepted)
        {
            return new BaseCommandResponse<AuthResponse>
            {
                Success = false,
                Message = "This invitation has already been accepted."
            };
        }
      
       
        if (request.Password != request.ConfirmPassword)
        {
            return new BaseCommandResponse<AuthResponse>
            {
                Success = false,
                Message = "Passwords do not match."
            };
        }
        var user = await userManager.FindByEmailAsync(invitation.Email);
        if (user != null)
        {
            return new BaseCommandResponse<AuthResponse>
            {
                Success = false,
                Message = "This email already has an account."
            };
        }
       
            user = new User
            {
                FirstName = invitation.FirstName,
                LastName = invitation.LastName,
                UserName = invitation.Email,
                Email = invitation.Email,
                TenantId = invitation.TenantId,
                EmailConfirmed = true,
                IsActive = true
            };

            var createResult = await userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                return new BaseCommandResponse<AuthResponse>
                {
                    Success = false,
                    Message = string.Join(", ", createResult.Errors.Select(x => x.Description))
                };
            }
        
          user.TenantId = invitation.TenantId;

            await userManager.AddPasswordAsync(user, request.Password);
            await userManager.UpdateAsync(user);
        await userManager.AddToRoleAsync(user, invitation.Role.ToString());
        invitation.IsAccepted = true;

        await unitOfWork.Invitations.UpdateAsync(invitation);
        await unitOfWork.CompleteAsync();
        var roles = await userManager.GetRolesAsync(user);

        var jwt = await jwtService.CreateJwtToken(user, roles);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

        var refreshToken = await refreshTokenService.GenerateRefreshTokenAsync(user);
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
            TokenExpiration = jwt.ValidTo,
            RefreshToken = refreshToken,
            RefreshTokenExpiration =
        DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenDurationInDays)
            }
        };
    }

}