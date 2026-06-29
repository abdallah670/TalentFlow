using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.WorkflowModule;

public class ApplicationStageHistory : BaseEntity
{
    public Guid ApplicationId { get; set; }
    public Guid? FromStageId { get; set; }
    public Guid ToStageId { get; set; }
    public Guid ChangedByUserId { get; set; }
    public string? Notes { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
