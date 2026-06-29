using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Persistence.Repositories;

public class CandidateSkillRepository : GenericRepository<CandidateSkill>, ICandidateSkillRepository
{
    public CandidateSkillRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
