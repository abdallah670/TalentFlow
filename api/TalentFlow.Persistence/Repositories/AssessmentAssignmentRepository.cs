using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.AssessmentModule;

namespace TalentFlow.Persistence.Repositories;

public class AssessmentAssignmentRepository : GenericRepository<AssessmentAssignment>, IAssessmentAssignmentRepository
{
    public AssessmentAssignmentRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
