
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TalentFlow.Application.Contracts;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Models;
using TalentFlow.Application.Models.Authentication;
using TalentFlow.Infrastructure.Service;
using TalentFlow.Persistence;

namespace TalentFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddScoped<IJWTService, JWTSErvice>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddHttpContextAccessor();
        services.AddScoped<IEmailService, EmailService>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
       services.AddScoped<ICurrentTenantService, CurrentTenantService>();
        services.AddScoped<IAuditService, AuditService>();

        return services;
    }
}
