using MediatR;
using TalentFlow.Application.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Models;
using TalentFlow.Application.Models.Identity;
using TalentFlow.Domain.Entities.IdentityModule;
using TalentFlow.Domain.Entities.TenantModule;
using TalentFlow.Domain.Entities.WorkflowModule;

namespace TalentFlow.Application.Features.Tenant.Command.RegisterTenant
{
    public class TenantRegisterCommandHandler : IRequestHandler<TenantRegisterCommand, BaseCommandResponse<AuthResponse>>
    {
        private readonly ILogger<TenantRegisterCommandHandler> logger;
        private readonly UserManager<Domain.Entities.IdentityModule.User> userManager;
        private readonly IJWTService jwtService;
        private readonly IRefreshTokenService refreshTokenService;
        private readonly JwtSettings jwtSettings;
        private readonly IUnitOfWork unitOfWork;
        private readonly AppUrlSettings appUrlSettings;
        private readonly IEmailService emailService;

        public TenantRegisterCommandHandler(
            UserManager<Domain.Entities.IdentityModule.User> userManager,
            IJWTService jwtService,
            IRefreshTokenService refreshTokenService,
            IOptions<JwtSettings> jwtSettings,
            IUnitOfWork unitOfWork,
            IOptions<AppUrlSettings> appUrlSettings,
            IEmailService emailService, ILogger<TenantRegisterCommandHandler> logger)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.jwtService = jwtService;
            this.refreshTokenService = refreshTokenService;
            this.jwtSettings = jwtSettings.Value;
            this.unitOfWork = unitOfWork;
            this.appUrlSettings = appUrlSettings.Value;
            this.emailService = emailService;
        }

        public async Task<BaseCommandResponse<AuthResponse>> Handle(TenantRegisterCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(TenantRegisterCommandHandler));
            var existingUser = await userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return new BaseCommandResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Email already exists."
                };
            }

            var existingTenant =
                await unitOfWork.Tenants.FindAsync(x => x.Name == request.TenantName);

            if (existingTenant.Any())
            {
                return new BaseCommandResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Tenant already exists."
                };
            }

            var existingSlug =
                await unitOfWork.Tenants.FindAsync(x => x.Slug == request.Slug);

            if (existingSlug.Any())
            {
                return new BaseCommandResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Slug already exists."
                };
            }

            var tenant = new Domain.Entities.TenantModule.Tenant
            {
                Name = request.TenantName,
                Slug = request.Slug,
                SubscriptionPlan = request.SubscriptionPlan,
                CompanySize = request.CompanySize,
                Industry = request.Industry,
                Website = request.Website,
                LinkedInUrl = request.LinkedIn,
                OfficeLocation = request.OfficeLocation,
                IsActive = true
            };

            await unitOfWork.Tenants.AddAsync(tenant);

            await unitOfWork.TenantSettings.AddAsync(new TenantSetting
            {
                TenantId = tenant.Id,
                TimeZone = "UTC",
                DateFormat = "dd/MM/yyyy"
            });

            var pipeline = new Pipeline
            {
                TenantId = tenant.Id,
                Name = "Default Hiring Pipeline"
            };

            await unitOfWork.Pipelines.AddAsync(pipeline);

            await unitOfWork.PipelineStages.AddAsync(new PipelineStage
            {
                PipelineId = pipeline.Id,
                Name = "Applied",
                StageOrder = 1
            });

            await unitOfWork.PipelineStages.AddAsync(new PipelineStage
            {
                PipelineId = pipeline.Id,
                Name = "Screening",
                StageOrder = 2
            });

            await unitOfWork.PipelineStages.AddAsync(new PipelineStage
            {
                PipelineId = pipeline.Id,
                Name = "Interview",
                StageOrder = 3
            });

            await unitOfWork.PipelineStages.AddAsync(new PipelineStage
            {
                PipelineId = pipeline.Id,
                Name = "Offer",
                StageOrder = 4
            });

            await unitOfWork.PipelineStages.AddAsync(new PipelineStage
            {
                PipelineId = pipeline.Id,
                Name = "Hired",
                StageOrder = 5
            });

            var user = new Domain.Entities.IdentityModule.User

            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Email = request.Email,
                TenantId = tenant.Id,
                IsActive = true
            };

            var createResult =
                await userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                await unitOfWork.Tenants.DeleteAsync(tenant);

                return new BaseCommandResponse<AuthResponse>
                {
                    Success = false,
                    Message = string.Join(", ",
                        createResult.Errors.Select(x => x.Description))
                };
            }

            var roleResult =
                await userManager.AddToRoleAsync(user, Domain.Enums.Roles.TenantAdmin.ToString());

            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(user);
                await unitOfWork.Tenants.DeleteAsync(tenant);

                return new BaseCommandResponse<AuthResponse>
                {
                    Success = false,
                    Message = string.Join(", ",
                        roleResult.Errors.Select(x => x.Description))
                };
            }

            await unitOfWork.CompleteAsync();

            var token =
                await userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedToken =
                Uri.EscapeDataString(token);

            var confirmationLink =
                $"{appUrlSettings.ApiBaseUrl}/api/Auth/ConfirmEmail?userId={user.Id}&token={encodedToken}";
            try
            {
                await emailService.SendEmailAsync(
                    user.Email!,
                    "Confirm your email",
                    $"""
                    <h2>Welcome {user.FirstName}</h2>

                    <p>Thank you for registering your company.</p>

                    <p>Please click the link below to confirm your email:</p>

                    <a href="{confirmationLink}">
                        Confirm Email
                    </a>
                    """);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Role assignment failed for new tenant user, continuing registration.");
            }

            var roles = await userManager.GetRolesAsync(user);

            return new BaseCommandResponse<AuthResponse>
            {
                Success = true,
                Message = "Registration successful. Please check your email to verify your account.",
                Data = new AuthResponse
                {
                    Id = user.Id.ToString(),
                    UserName = user.UserName!,
                    Email = user.Email!,
                    Roles = roles.ToList(),
                    IsAuthenticated = false
                }
            };
        }
    }
}