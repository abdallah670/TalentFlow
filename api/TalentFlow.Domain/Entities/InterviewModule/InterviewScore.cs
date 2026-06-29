using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.InterviewModule;

public class InterviewScore : BaseEntity
{
    public Guid FeedbackId { get; set; }
    public Guid CriteriaId { get; set; }
    public int Score { get; set; }
}
