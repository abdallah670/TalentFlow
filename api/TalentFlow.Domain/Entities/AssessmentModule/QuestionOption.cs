using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.AssessmentModule;

public class QuestionOption : BaseEntity
{
    public Guid QuestionId { get; set; }
    public string OptionText { get; set; } = default!;
    public bool IsCorrect { get; set; }
}
