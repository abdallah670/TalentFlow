using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.TenantModule;

public class Tenant : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }

    public string Name { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string? SubscriptionPlan { get; set; }
}
