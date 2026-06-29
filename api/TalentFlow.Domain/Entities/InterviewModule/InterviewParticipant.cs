using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.InterviewModule;

public class InterviewParticipant : BaseEntity
{
    public Guid InterviewId { get; set; }
    public Guid UserId { get; set; }
    public string Role { get; set; } = default!;
}
