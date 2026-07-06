using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Persistence.Repositories;

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
