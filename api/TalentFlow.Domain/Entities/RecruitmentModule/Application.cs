using System;
using TalentFlow.Domain.Common;
using TalentFlow.Domain.Entities.CandidateModule;

namespace TalentFlow.Domain.Entities.RecruitmentModule;

public class Application : BaseEntity, ITenantEntity, ISoftDelete
{
    public Guid TenantId { get; set; }

    public Guid CandidateProfileId { get; set; }
    public CandidateProfile CandidateProfile { get; set; } = null!;
    public Guid JobId { get; set; }
    public Guid CurrentStageId { get; set; }
    public string Status { get; set; } = default!;
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}
