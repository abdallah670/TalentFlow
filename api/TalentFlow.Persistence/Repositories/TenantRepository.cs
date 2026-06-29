using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.TenantModule;

namespace TalentFlow.Persistence.Repositories;

public class TenantRepository : GenericRepository<Tenant>, ITenantRepository
{
    public TenantRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
