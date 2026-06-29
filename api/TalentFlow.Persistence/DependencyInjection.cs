using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Persistence.Repositories;

namespace TalentFlow.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<ITenantSettingRepository, TenantSettingRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IJobSkillRepository, JobSkillRepository>();
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddScoped<ICandidateSkillRepository, CandidateSkillRepository>();
        services.AddScoped<ICandidateExperienceRepository, CandidateExperienceRepository>();
        services.AddScoped<ICandidateEducationRepository, CandidateEducationRepository>();
        services.AddScoped<ICandidateDocumentRepository, CandidateDocumentRepository>();
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<IPipelineRepository, PipelineRepository>();
        services.AddScoped<IPipelineStageRepository, PipelineStageRepository>();
        services.AddScoped<IApplicationStageHistoryRepository, ApplicationStageHistoryRepository>();
        services.AddScoped<ICandidateActivityRepository, CandidateActivityRepository>();
        services.AddScoped<IInterviewRepository, InterviewRepository>();
        services.AddScoped<IInterviewParticipantRepository, InterviewParticipantRepository>();
        services.AddScoped<IInterviewCriteriaRepository, InterviewCriteriaRepository>();
        services.AddScoped<IInterviewFeedbackRepository, InterviewFeedbackRepository>();
        services.AddScoped<IInterviewScoreRepository, InterviewScoreRepository>();
        services.AddScoped<IAssessmentRepository, AssessmentRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IQuestionOptionRepository, QuestionOptionRepository>();
        services.AddScoped<IAssessmentAssignmentRepository, AssessmentAssignmentRepository>();
        services.AddScoped<ICandidateAnswerRepository, CandidateAnswerRepository>();
        services.AddScoped<IAssessmentResultRepository, AssessmentResultRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<INotificationTemplateRepository, NotificationTemplateRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();

        return services;
    }
}
