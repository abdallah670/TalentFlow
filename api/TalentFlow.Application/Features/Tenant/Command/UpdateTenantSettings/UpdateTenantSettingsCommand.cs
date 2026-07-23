using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Tenant.Command.UpdateTenantSettings
{
    public class UpdateTenantSettingsCommand :IRequest<BaseCommandResponse>
    {
        // Tenant
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? SubscriptionPlan { get; set; }

        // TenantSettings
        public string? CompanyLogoUrl { get; set; }
        public string? PrimaryColor { get; set; }
        public string? TimeZone { get; set; }
        public string? DateFormat { get; set; }
    }
}
