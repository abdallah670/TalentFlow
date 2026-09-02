using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.TenantModule;

namespace TalentFlow.Application.Features.Tenant.Command.UpdateTenantSettings
{
    public class UpdateTenantSettingsCommandHandler : IRequestHandler<UpdateTenantSettingsCommand, BaseCommandResponse<bool>>
    {
        private readonly ILogger<UpdateTenantSettingsCommandHandler> logger;
        private readonly ICurrentUserService currentUserService;
        private readonly ITenantRepository tenantRepository;

        private readonly ITenantSettingRepository tenantSettingRepository;

        public UpdateTenantSettingsCommandHandler(ICurrentUserService currentUserService, ITenantRepository tenantRepository, ITenantSettingRepository tenantSettingRepository, ILogger<UpdateTenantSettingsCommandHandler> logger)
        {
            this.logger = logger;
            this.currentUserService = currentUserService;
            this.tenantRepository = tenantRepository;
            this.tenantSettingRepository = tenantSettingRepository;
        }

        public async Task<BaseCommandResponse<bool>> Handle(UpdateTenantSettingsCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(UpdateTenantSettingsCommandHandler));
            var tenants = await tenantRepository.FindAsync(x => x.Id == currentUserService.TenantId);
            var tenant = tenants.FirstOrDefault();

            var tenantSettings = await tenantSettingRepository.FindAsync(x => x.TenantId == currentUserService.TenantId);
            var tenantSetting = tenantSettings.FirstOrDefault();
            if (tenant == null || tenantSettings == null)
            {
                throw new Exception("Tenant not found");
            }

            tenant.Name = request.Name;
            tenant.Slug = request.Slug;
            tenant.SubscriptionPlan = request.SubscriptionPlan;

            tenantSetting.CompanyLogoUrl = request.CompanyLogoUrl;
            tenantSetting.PrimaryColor = request.PrimaryColor;
            tenantSetting.TimeZone = request.TimeZone;
            tenantSetting.DateFormat = request.DateFormat;
            await tenantRepository.UpdateAsync(tenant);
            await tenantSettingRepository.UpdateAsync(tenantSetting);
            return new BaseCommandResponse<bool>
            {
                Success = true,
                Message = "Tenant settings updated successfully."
            };
        }
    }
}
