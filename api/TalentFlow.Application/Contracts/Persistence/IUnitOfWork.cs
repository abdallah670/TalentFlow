using System;
using System.Threading.Tasks;

namespace TalentFlow.Application.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
    ITenantRepository Tenants { get; }
    ITenantSettingRepository TenantSettings { get; }
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IPermissionRepository Permissions { get; }
    IUserRoleRepository UserRoles { get; }
    IRolePermissionRepository RolePermissions { get; }
    IDepartmentRepository Departments { get; }
    ISkillRepository Skills { get; }
    IJobRepository Jobs { get; }
    IJobSkillRepository JobSkills { get; }
    ICandidateRepository Candidates { get; }
    ICandidateSkillRepository CandidateSkills { get; }
    ICandidateExperienceRepository CandidateExperiences { get; }
    ICandidateEducationRepository CandidateEducations { get; }
    ICandidateDocumentRepository CandidateDocuments { get; }
    IApplicationRepository Applications { get; }
    IPipelineRepository Pipelines { get; }
    IPipelineStageRepository PipelineStages { get; }
    IApplicationStageHistoryRepository ApplicationStageHistorys { get; }
    ICandidateActivityRepository CandidateActivitys { get; }
    IInterviewRepository Interviews { get; }
    IInterviewParticipantRepository InterviewParticipants { get; }
    IInterviewCriteriaRepository InterviewCriterias { get; }
    IInterviewFeedbackRepository InterviewFeedbacks { get; }
    IInterviewScoreRepository InterviewScores { get; }
    IAssessmentRepository Assessments { get; }
    IQuestionRepository Questions { get; }
    IQuestionOptionRepository QuestionOptions { get; }
    IAssessmentAssignmentRepository AssessmentAssignments { get; }
    ICandidateAnswerRepository CandidateAnswers { get; }
    IAssessmentResultRepository AssessmentResults { get; }
    INotificationRepository Notifications { get; }
    INotificationTemplateRepository NotificationTemplates { get; }
    IAuditLogRepository AuditLogs { get; }
    Task<int> CompleteAsync();
}
