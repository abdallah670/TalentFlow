using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.WorkflowModule;

public class CandidateActivity : BaseEntity
{
    public Guid CandidateId { get; set; }
    public string ActivityType { get; set; } = default!;
    public string? Description { get; set; }
    public Guid CreatedByUserId { get; set; }
}
