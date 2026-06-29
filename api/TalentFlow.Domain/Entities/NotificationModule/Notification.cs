using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.NotificationModule;

public class Notification : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }

    public Guid UserId { get; set; }
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public bool IsRead { get; set; }
}
