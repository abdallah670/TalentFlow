using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TalentFlow.Application.Contracts;

using TalentFlow.Application.Models;
using TalentFlow.Application.Models.Authentication;

using TalentFlow.Persistence;

namespace TalentFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
       
        return services;
    }
}
