using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.InterviewModule;

namespace TalentFlow.Persistence.Repositories;

public class InterviewCriteriaRepository : GenericRepository<InterviewCriteria>, IInterviewCriteriaRepository
{
    public InterviewCriteriaRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
