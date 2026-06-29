using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.RecruitmentModule;

public class CandidateEducation : BaseEntity
{
    public Guid CandidateId { get; set; }
    public string Institution { get; set; } = default!;
    public string Degree { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
