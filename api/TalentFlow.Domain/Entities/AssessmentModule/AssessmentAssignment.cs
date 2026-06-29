using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.AssessmentModule;

public class AssessmentAssignment : BaseEntity
{
    public Guid AssessmentId { get; set; }
    public Guid ApplicationId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
}
