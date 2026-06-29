using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.WorkflowModule;

namespace TalentFlow.Persistence.Repositories;

public class PipelineStageRepository : GenericRepository<PipelineStage>, IPipelineStageRepository
{
    public PipelineStageRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
