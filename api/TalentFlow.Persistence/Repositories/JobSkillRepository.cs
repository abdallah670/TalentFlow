using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Persistence.Repositories;

public class JobSkillRepository : GenericRepository<JobSkill>, IJobSkillRepository
{
    public JobSkillRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
