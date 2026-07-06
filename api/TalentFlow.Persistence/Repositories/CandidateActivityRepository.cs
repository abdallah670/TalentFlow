using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.WorkflowModule;

namespace TalentFlow.Persistence.Repositories;

public class CandidateActivityRepository : GenericRepository<CandidateActivity>, ICandidateActivityRepository
{
    public CandidateActivityRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
