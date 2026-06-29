using TalentFlow.Domain.Entities.AuditModule;

namespace TalentFlow.Application.Contracts.Persistence;

public interface IAuditLogRepository : IGenericRepository<AuditLog>
{
    // Add custom repository methods for AuditLog here
}
