using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.InterviewModule;

namespace TalentFlow.Persistence.Repositories;

public class InterviewFeedbackRepository : GenericRepository<InterviewFeedback>, IInterviewFeedbackRepository
{
    public InterviewFeedbackRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
