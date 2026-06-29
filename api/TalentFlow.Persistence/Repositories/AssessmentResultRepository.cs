using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.AssessmentModule;

namespace TalentFlow.Persistence.Repositories;

public class AssessmentResultRepository : GenericRepository<AssessmentResult>, IAssessmentResultRepository
{
    public AssessmentResultRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
