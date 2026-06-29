using TalentFlow.Domain.Entities.TenantModule;

namespace TalentFlow.Application.Contracts.Persistence;

public interface ITenantRepository : IGenericRepository<Tenant>
{
    // Add custom repository methods for Tenant here
}
