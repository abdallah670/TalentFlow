using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Contracts.Persistence;

public interface IUserRepository : IGenericRepository<User>
{
    // Add custom repository methods for User here
}
