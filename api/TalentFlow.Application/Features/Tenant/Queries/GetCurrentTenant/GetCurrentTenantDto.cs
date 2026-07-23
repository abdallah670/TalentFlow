using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Tenant.Queries.GetCurrentTenant
{
    public class GetCurrentTenantDto
    {
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? SubscriptionPlan { get; set; }

        public string? CompanyLogoUrl { get; set; }
        public string? PrimaryColor { get; set; }
        public string? TimeZone { get; set; }
        public string? DateFormat { get; set; }
    }
}
