using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.RecruitmentModule;

public class CandidateExperience : BaseEntity
{
    public Guid CandidateId { get; set; }
    public string CompanyName { get; set; } = default!;
    public string JobTitle { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
}
