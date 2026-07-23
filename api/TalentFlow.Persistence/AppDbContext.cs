using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TalentFlow.Application.Interfaces;
using TalentFlow.Domain.Common;
using TalentFlow.Domain.Entities.AssessmentModule;
using TalentFlow.Domain.Entities.AuditModule;
using TalentFlow.Domain.Entities.IdentityModule;
using TalentFlow.Domain.Entities.InterviewModule;
using TalentFlow.Domain.Entities.NotificationModule;
using TalentFlow.Domain.Entities.OfferModule;
using TalentFlow.Domain.Entities.RecruitmentModule;
using TalentFlow.Domain.Entities.TenantModule;
using TalentFlow.Domain.Entities.WorkflowModule;

namespace TalentFlow.Persistence;

public class AppDbContext : IdentityDbContext<User, Role, Guid>
{
    private readonly ICurrentTenantService _currentTenantService;

    private readonly IAuditService _auditService;
    public AppDbContext(
     DbContextOptions<AppDbContext> options,
     ICurrentTenantService currentTenantService,
     IAuditService auditService)
     : base(options)
    {
        _currentTenantService = currentTenantService;
        _auditService = auditService;
    }
    public DbSet<Tenant> Tenants { get; set; } = null!;
    public DbSet<TenantSetting> TenantSettings { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Skill> Skills { get; set; } = null!;
    public DbSet<Job> Jobs { get; set; } = null!;
    public DbSet<JobSkill> JobSkills { get; set; } = null!;
    public DbSet<Candidate> Candidates { get; set; } = null!;
    public DbSet<CandidateSkill> CandidateSkills { get; set; } = null!;
    public DbSet<CandidateExperience> CandidateExperiences { get; set; } = null!;
    public DbSet<CandidateEducation> CandidateEducations { get; set; } = null!;
    public DbSet<CandidateDocument> CandidateDocuments { get; set; } = null!;
    public DbSet<TalentFlow.Domain.Entities.RecruitmentModule.Application> Applications { get; set; } = null!;
    public DbSet<Pipeline> Pipelines { get; set; } = null!;
    public DbSet<PipelineStage> PipelineStages { get; set; } = null!;
    public DbSet<ApplicationStageHistory> ApplicationStageHistorys { get; set; } = null!;
    public DbSet<CandidateActivity> CandidateActivitys { get; set; } = null!;
    public DbSet<Interview> Interviews { get; set; } = null!;
    public DbSet<InterviewParticipant> InterviewParticipants { get; set; } = null!;
    public DbSet<InterviewCriteria> InterviewCriterias { get; set; } = null!;
    public DbSet<InterviewFeedback> InterviewFeedbacks { get; set; } = null!;
    public DbSet<InterviewScore> InterviewScores { get; set; } = null!;
    public DbSet<Assessment> Assessments { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<QuestionOption> QuestionOptions { get; set; } = null!;
    public DbSet<AssessmentAssignment> AssessmentAssignments { get; set; } = null!;
    public DbSet<CandidateAnswer> CandidateAnswers { get; set; } = null!;
    public DbSet<AssessmentResult> AssessmentResults { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<NotificationTemplate> NotificationTemplates { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;
    public DbSet<Offer>  offers { get; set; } = default!;
    public DbSet<OfferApproval> offerApprovals { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Apply configurations from assembly
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        builder.Entity<Job>()
    .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<Candidate>()
            .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<Department>()
            .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);
        builder.Entity<Offer>()
                  .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);
        builder.Entity<OfferApproval>()
          .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<AuditLog>()
          .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<Notification>()
          .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<NotificationTemplate>()
          .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<Department>()
          .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<Tenant>()
    .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<TenantSetting>()
            .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<Department>()
            .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<Job>()
            .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<Candidate>()
            .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);


        builder.Entity<Pipeline>()
            .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<InterviewCriteria>()
            .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<Assessment>()
            .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);



        builder.Entity<Notification>()
            .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<NotificationTemplate>()
            .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<AuditLog>()
            .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<Offer>()
            .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<OfferApproval>()
            .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        builder.Entity<Pipeline>()
            .HasQueryFilter(x => x.TenantId == _currentTenantService.TenantId);

        // Apply global query filter for soft delete on all ISoftDelete subtypes
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(AppDbContext)
                    .GetMethod(nameof(SetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    ?.MakeGenericMethod(entityType.ClrType);
                method?.Invoke(null, new object[] { builder });
            }
        }
    }

    private static void SetSoftDeleteFilter<TEntity>(ModelBuilder builder) where TEntity : class, ISoftDelete
    {
        builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity baseEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    baseEntity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    baseEntity.UpdatedAt = DateTime.UtcNow;
                }
            }
            else if (entry.Entity is User userEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    userEntity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    userEntity.UpdatedAt = DateTime.UtcNow;
                }
            }
            else if (entry.Entity is Role roleEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    roleEntity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    roleEntity.UpdatedAt = DateTime.UtcNow;
                }
            }

            if (entry.Entity is ISoftDelete softDeleteEntity && entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                softDeleteEntity.IsDeleted = true;
                softDeleteEntity.DeletedAt = DateTime.UtcNow;
            }
        }
        await _auditService.CreateAuditLogsAsync(ChangeTracker);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
