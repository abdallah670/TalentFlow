using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Persistence.Repositories;

public class CandidateRepository : GenericRepository<Candidate>, ICandidateRepository
{
    public CandidateRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
