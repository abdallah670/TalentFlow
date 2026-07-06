using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.InterviewModule;

namespace TalentFlow.Persistence.Repositories;

public class InterviewScoreRepository : GenericRepository<InterviewScore>, IInterviewScoreRepository
{
    public InterviewScoreRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
