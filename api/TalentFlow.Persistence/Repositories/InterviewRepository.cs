using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.InterviewModule;

namespace TalentFlow.Persistence.Repositories;

public class InterviewRepository : GenericRepository<Interview>, IInterviewRepository
{
    public InterviewRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
