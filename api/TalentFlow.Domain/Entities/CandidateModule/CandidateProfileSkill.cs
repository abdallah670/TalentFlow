using TalentFlow.Domain.Common;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Domain.Entities.CandidateModule
{
    public class CandidateProfileSkill : BaseEntity
    {
        public Guid CandidateProfileId { get; set; }
        public CandidateProfile CandidateProfile { get; set; } = null!;

        public Guid SkillId { get; set; }
        public Skill Skill { get; set; } = null!;
    }
}