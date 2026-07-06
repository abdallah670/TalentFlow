using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.TenantModule;

namespace TalentFlow.Persistence.Repositories;

public class TenantSettingRepository : GenericRepository<TenantSetting>, ITenantSettingRepository
{
    public TenantSettingRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
