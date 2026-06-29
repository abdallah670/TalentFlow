using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.InterviewModule;

public class InterviewCriteria : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }

    public string Name { get; set; } = default!;
    public int MaxScore { get; set; }
}
