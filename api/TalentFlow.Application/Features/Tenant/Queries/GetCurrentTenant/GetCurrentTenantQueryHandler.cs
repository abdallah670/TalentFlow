using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Tenant.Queries.GetCurrentTenant
{
    public class GetCurrentTenantQueryHandler : IRequestHandler<GetCurrentTenantQuery, BaseCommandResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantSettingRepository _tenantSettingRepository;

        public GetCurrentTenantQueryHandler(
            ICurrentUserService currentUserService,
            ITenantRepository tenantRepository,
            ITenantSettingRepository tenantSettingRepository)
        {
            _currentUserService = currentUserService;
            _tenantRepository = tenantRepository;
            _tenantSettingRepository = tenantSettingRepository;
        }
        public async Task<BaseCommandResponse> Handle(GetCurrentTenantQuery request, CancellationToken cancellationToken)
        {
            var tenant = (await _tenantRepository
                           .FindAsync(x => x.TenantId == _currentUserService.TenantId))
                           .FirstOrDefault();

            var settings = (await _tenantSettingRepository
                .FindAsync(x => x.TenantId == _currentUserService.TenantId))
                .FirstOrDefault();

            if (tenant == null)
                throw new Exception("Tenant not found");

            var res = new GetCurrentTenantDto
            {
                Name = tenant.Name,
                Slug = tenant.Slug,
                SubscriptionPlan = tenant.SubscriptionPlan,
                CompanyLogoUrl = settings?.CompanyLogoUrl,
                PrimaryColor = settings?.PrimaryColor,
                TimeZone = settings?.TimeZone,
                DateFormat = settings?.DateFormat
            };
            return new BaseCommandResponse
            {
                Success = true,
                Message = "Tenant retrieved successfully.",
                Data = res
            };

        }
    }
}
