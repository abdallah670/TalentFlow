using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.WorkflowModule;

namespace TalentFlow.Persistence.Repositories;

public class ApplicationStageHistoryRepository : GenericRepository<ApplicationStageHistory>, IApplicationStageHistoryRepository
{
    public ApplicationStageHistoryRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
