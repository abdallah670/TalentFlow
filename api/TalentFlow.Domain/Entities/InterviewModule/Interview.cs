using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.InterviewModule;

public class Interview : BaseEntity, ISoftDelete
{
    public Guid ApplicationId { get; set; }
    public string InterviewType { get; set; } = default!;
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }
    public string Status { get; set; } = default!;

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}
