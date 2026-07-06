using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.AssessmentModule;

namespace TalentFlow.Persistence.Repositories;

public class QuestionOptionRepository : GenericRepository<QuestionOption>, IQuestionOptionRepository
{
    public QuestionOptionRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
