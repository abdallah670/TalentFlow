using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.WorkflowModule;

namespace TalentFlow.Persistence.Repositories;

public class PipelineRepository : GenericRepository<Pipeline>, IPipelineRepository
{
    public PipelineRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
