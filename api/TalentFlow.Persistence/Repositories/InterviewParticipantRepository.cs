using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.InterviewModule;

namespace TalentFlow.Persistence.Repositories;

public class InterviewParticipantRepository : GenericRepository<InterviewParticipant>, IInterviewParticipantRepository
{
    public InterviewParticipantRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
