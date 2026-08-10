using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.TenantModule
{
    public class Tenant : BaseEntity
    {
        public string Name { get; set; } = default!;

        public string Slug { get; set; } = default!;

        public string CompanySize { get; set; } = default!;

        public string Industry { get; set; } = default!;

        public string? Website { get; set; }

        public string? LinkedInUrl { get; set; }

        public string? OfficeLocation { get; set; }

        public string SubscriptionPlan { get; set; } = "Free";

        public bool IsActive { get; set; } = true;
    }
}