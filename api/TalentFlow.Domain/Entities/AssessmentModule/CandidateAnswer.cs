using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.AssessmentModule;

public class CandidateAnswer : BaseEntity
{
    public Guid AssignmentId { get; set; }
    public Guid QuestionId { get; set; }
    public string AnswerText { get; set; } = default!;
}
