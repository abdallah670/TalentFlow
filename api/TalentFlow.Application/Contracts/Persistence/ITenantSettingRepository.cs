using TalentFlow.Domain.Entities.TenantModule;

namespace TalentFlow.Application.Contracts.Persistence;

public interface ITenantSettingRepository : IGenericRepository<TenantSetting>
{
    // Add custom repository methods for TenantSetting here
}
