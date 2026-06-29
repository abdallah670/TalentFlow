using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.AssessmentModule;

public class Question : BaseEntity
{
    public Guid AssessmentId { get; set; }
    public string QuestionType { get; set; } = default!;
    public string Content { get; set; } = default!;
    public int Points { get; set; }
}
