using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.AssessmentModule;

public class Assessment : BaseEntity, ITenantEntity, ISoftDelete
{
    public Guid TenantId { get; set; }

    public Guid JobId { get; set; }
    public string Title { get; set; } = default!;
    public int PassingScore { get; set; }
    public int DurationMinutes { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}
