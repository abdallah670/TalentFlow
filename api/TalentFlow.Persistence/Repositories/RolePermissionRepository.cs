using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Persistence.Repositories;

public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
{
    public RolePermissionRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
