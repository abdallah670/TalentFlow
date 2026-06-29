using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.RecruitmentModule;

public class CandidateSkill : BaseEntity
{
    public Guid CandidateId { get; set; }
    public Guid SkillId { get; set; }
    public byte SkillLevel { get; set; }
}
