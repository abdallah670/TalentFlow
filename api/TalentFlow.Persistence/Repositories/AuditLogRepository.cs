using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.AuditModule;

namespace TalentFlow.Persistence.Repositories;

public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
