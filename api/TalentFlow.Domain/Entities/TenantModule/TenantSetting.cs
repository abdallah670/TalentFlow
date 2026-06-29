using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.TenantModule;

public class TenantSetting : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }

    public string? CompanyLogoUrl { get; set; }
    public string? PrimaryColor { get; set; }
    public string? TimeZone { get; set; }
    public string? DateFormat { get; set; }
}
