using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.AssessmentModule;

namespace TalentFlow.Persistence.Repositories;

public class AssessmentRepository : GenericRepository<Assessment>, IAssessmentRepository
{
    public AssessmentRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
