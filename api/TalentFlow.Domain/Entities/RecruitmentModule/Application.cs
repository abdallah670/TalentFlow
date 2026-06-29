using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.RecruitmentModule;

public class Application : BaseEntity, ITenantEntity, ISoftDelete
{
    public Guid TenantId { get; set; }

    public Guid CandidateId { get; set; }
    public Guid JobId { get; set; }
    public Guid CurrentStageId { get; set; }
    public string Status { get; set; } = default!;
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}
