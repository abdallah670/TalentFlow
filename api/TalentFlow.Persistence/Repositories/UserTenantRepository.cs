using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.TenantModule;

namespace TalentFlow.Persistence.Repositories;

public class UserTenantRepository : GenericRepository<UserTenant>, IUserTenantRepository
{
    public UserTenantRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}