using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.RecruitmentModule;

public class JobSkill : BaseEntity
{
    public Guid JobId { get; set; }
    public Guid SkillId { get; set; }
    public byte RequiredLevel { get; set; }
    public bool IsMandatory { get; set; }
}
