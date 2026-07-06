using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Persistence.Repositories;

public class CandidateExperienceRepository : GenericRepository<CandidateExperience>, ICandidateExperienceRepository
{
    public CandidateExperienceRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
