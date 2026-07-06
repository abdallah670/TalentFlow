using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Persistence.Repositories;

public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
{
    public PermissionRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
