using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.AssessmentModule;

public class AssessmentResult : BaseEntity
{
    public Guid AssignmentId { get; set; }
    public int Score { get; set; }
    public bool Passed { get; set; }
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
}
