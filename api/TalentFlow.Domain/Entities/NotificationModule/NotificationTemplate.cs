using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.NotificationModule;

public class NotificationTemplate : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }

    public string Name { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
}
