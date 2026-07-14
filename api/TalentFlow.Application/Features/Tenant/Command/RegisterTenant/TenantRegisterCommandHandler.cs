using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Xml.Schema;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Models.Identity;
using TalentFlow.Domain.Entities.IdentityModule;
using TalentFlow.Domain.Entities.TenantModule;
using TalentFlow.Domain.Enums;

namespace TalentFlow.Application.Features.Tenant.Command.RegisterTenant
{
    public class TenantRegisterCommandHandler : IRequestHandler<TenantRegisterCommand, AuthResponse>
    {
        private readonly UserManager<Domain.Entities.IdentityModule.User> userManager;
        private readonly IJWTService jWTService;
        private readonly IRefreshTokenService refreshTokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly ITenantRepository _tenantRepository;


        public TenantRegisterCommandHandler(
    UserManager<Domain.Entities.IdentityModule.User> userManager,
    IJWTService jWTService,
    IRefreshTokenService refreshTokenService,
    IOptions<JwtSettings> jwtSettings,
    ITenantRepository tenantRepository)
        {
            this.userManager = userManager;
            this.jWTService = jWTService;
            this.refreshTokenService = refreshTokenService;
            _jwtSettings = jwtSettings.Value;
            _tenantRepository = tenantRepository;   
        }


        public async Task<AuthResponse> Handle(TenantRegisterCommand request, CancellationToken cancellationToken)
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

            var existingTenant = await _tenantRepository.FindAsync(x => x.Name == request.TentantName);
            if (existingTenant.Any()  )
            {
                return new AuthResponse { IsAuthenticated = false, Message = "Tenant Already Exist" };
            }



            var existingSlug = await _tenantRepository.FindAsync(x => x.Slug == request.Slug);

            if (existingSlug.Any())
            {
                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message = "Slug already exists."
                };
            }



                var tenant = new Domain.Entities.TenantModule.Tenant
                {
                    Name = request.TentantName,
                    Slug = request.Slug,
                    IsActive = true

                };
                await _tenantRepository.AddAsync(tenant);
                var user = new Domain.Entities.IdentityModule.User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    UserName = request.UserName,
                    IsActive = true,
                    TenantId = tenant.Id

                };


                var result = await userManager.CreateAsync(user, request.Password);


                if (!result.Succeeded)
                {
                await _tenantRepository.DeleteAsync(tenant);

                return new AuthResponse
                    {
                        IsAuthenticated = false,
                        Message = string.Join(", ", result.Errors.Select(x => x.Description))
                    };
                }

            var roleResult = await userManager.AddToRoleAsync(user, Roles.TenantAdmin.ToString());

            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(user);
                await _tenantRepository.DeleteAsync(tenant);

                return new AuthResponse
                {
                    IsAuthenticated = false,
                    Message = string.Join(", ", roleResult.Errors.Select(x => x.Description))
                };
            }
            var roles = await userManager.GetRolesAsync(user);
                var JwtToken = await jWTService.CreateJwtToken(user, roles);
                var accessToken = new JwtSecurityTokenHandler().WriteToken(JwtToken);
                var refreshToken = await refreshTokenService.GenerateRefreshTokenAsync(user);

                return new AuthResponse
                {
                    Id = user.Id.ToString(),
                    UserName = user.UserName!,
                    Email = user.Email!,
                    IsAuthenticated = true,
                    Roles = roles.ToList(),

                    Token = accessToken,
                    TokenExpiration = JwtToken.ValidTo,

                    RefreshToken = refreshToken,
                    RefreshTokenExpiration =
        DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationInDays)
                };
            }
            
        }
    }





