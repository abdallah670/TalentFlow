using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TalentFlow.Application.Interfaces;
using TalentFlow.Domain.Entities.AuditModule;
using TalentFlow.Persistence;

namespace TalentFlow.Infrastructure.Service
{
    public class AuditService : IAuditService
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AuditService(
            AppDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task CreateAuditLogsAsync(ChangeTracker changeTracker)
        {
            var auditLogs = new List<AuditLog>();

            foreach (var entry in changeTracker.Entries())
            {
                if (entry.Entity is AuditLog)
                    continue;

                if (entry.State != EntityState.Added &&
                    entry.State != EntityState.Modified &&
                    entry.State != EntityState.Deleted)
                    continue;

                var audit = new AuditLog
                {
                    TenantId = _currentUserService.TenantId,
                    UserId = _currentUserService.UserId,
                    EntityName = entry.Entity.GetType().Name,
                    EntityId = entry.Properties
                        .First(p => p.Metadata.IsPrimaryKey())
                        .CurrentValue?.ToString() ?? "",
                    Action = entry.State.ToString()
                };

                if (entry.State == EntityState.Added)
                {
                    audit.NewValues = JsonSerializer.Serialize(
                        entry.CurrentValues.Properties.ToDictionary(
                            p => p.Name,
                            p => entry.CurrentValues[p]));
                }

                if (entry.State == EntityState.Modified)
                {
                    audit.OldValues = JsonSerializer.Serialize(
                        entry.OriginalValues.Properties.ToDictionary(
                            p => p.Name,
                            p => entry.OriginalValues[p]));

                    audit.NewValues = JsonSerializer.Serialize(
                        entry.CurrentValues.Properties.ToDictionary(
                            p => p.Name,
                            p => entry.CurrentValues[p]));
                }

                if (entry.State == EntityState.Deleted)
                {
                    audit.OldValues = JsonSerializer.Serialize(
                        entry.OriginalValues.Properties.ToDictionary(
                            p => p.Name,
                            p => entry.OriginalValues[p]));
                }

                auditLogs.Add(audit);
            }

            if (auditLogs.Any())
            {
                await _context.AuditLogs.AddRangeAsync(auditLogs);
            }
        }
    }
}