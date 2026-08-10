using Microsoft.EntityFrameworkCore.ChangeTracking;
using TalentFlow.Domain.Entities.AuditModule;

namespace TalentFlow.Application.Interfaces
{
    public interface IAuditService
    {
        List<AuditLog> BuildAuditLogs(ChangeTracker changeTracker);
    }
}