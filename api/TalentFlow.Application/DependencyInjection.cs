
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TalentFlow.Application.Contracts;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Models;

namespace TalentFlow.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.Configure<EmailSettings>(
     configuration.GetSection("EmailSettings"));
            services.Configure<AppUrlSettings>(configuration.GetSection("AppSettings"));



            return services;
        }
    }
}
