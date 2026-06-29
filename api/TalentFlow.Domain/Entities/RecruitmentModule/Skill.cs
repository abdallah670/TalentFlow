using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.RecruitmentModule;

public class Skill : BaseEntity
{
    public string Name { get; set; } = default!;
}
