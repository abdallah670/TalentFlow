using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.InterviewModule;

public class InterviewFeedback : BaseEntity
{
    public Guid InterviewId { get; set; }
    public Guid SubmittedByUserId { get; set; }
    public string Recommendation { get; set; } = default!;
    public string? Summary { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}
