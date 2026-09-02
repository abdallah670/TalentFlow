using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Domain.Entities.AuditModule;

namespace TalentFlow.Persistence
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../TalentFlow.Api"))
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("TalentFlowConnection");
            builder.UseSqlServer(connectionString);

            return new AppDbContext(
                builder.Options,
                new DesignTimeCurrentTenantService(),
                new DesignTimeAuditService());
        }
    }

    internal class DesignTimeCurrentTenantService : ICurrentTenantService
    {
        public Guid TenantId => Guid.Empty;
    }

    internal class DesignTimeAuditService : IAuditService
    {
        public List<AuditLog> BuildAuditLogs(ChangeTracker changeTracker) => new List<AuditLog>();
    }
}