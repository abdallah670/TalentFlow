using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;

namespace TalentFlow.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    private ITenantRepository? _tenantRepository;
    private ITenantSettingRepository? _tenantsettingRepository;
    private IUserRepository? _userRepository;
    private IRoleRepository? _roleRepository;
    private IPermissionRepository? _permissionRepository;
    private IUserRoleRepository? _userroleRepository;
    private IRolePermissionRepository? _rolepermissionRepository;
    private IDepartmentRepository? _departmentRepository;
    private ISkillRepository? _skillRepository;
    private IJobRepository? _jobRepository;
    private IJobSkillRepository? _jobskillRepository;
    private ICandidateRepository? _candidateRepository;
    private ICandidateSkillRepository? _candidateskillRepository;
    private ICandidateExperienceRepository? _candidateexperienceRepository;
    private ICandidateEducationRepository? _candidateeducationRepository;
    private ICandidateDocumentRepository? _candidatedocumentRepository;
    private IApplicationRepository? _applicationRepository;
    private IPipelineRepository? _pipelineRepository;
    private IPipelineStageRepository? _pipelinestageRepository;
    private IApplicationStageHistoryRepository? _applicationstagehistoryRepository;
    private ICandidateActivityRepository? _candidateactivityRepository;
    private IInterviewRepository? _interviewRepository;
    private IInterviewParticipantRepository? _interviewparticipantRepository;
    private IInterviewCriteriaRepository? _interviewcriteriaRepository;
    private IInterviewFeedbackRepository? _interviewfeedbackRepository;
    private IInterviewScoreRepository? _interviewscoreRepository;
    private IAssessmentRepository? _assessmentRepository;
    private IQuestionRepository? _questionRepository;
    private IQuestionOptionRepository? _questionoptionRepository;
    private IAssessmentAssignmentRepository? _assessmentassignmentRepository;
    private ICandidateAnswerRepository? _candidateanswerRepository;
    private IAssessmentResultRepository? _assessmentresultRepository;
    private INotificationRepository? _notificationRepository;
    private INotificationTemplateRepository? _notificationtemplateRepository;
    private IAuditLogRepository? _auditlogRepository;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ITenantRepository Tenants => _tenantRepository ??= new TenantRepository(_dbContext);
    public ITenantSettingRepository TenantSettings => _tenantsettingRepository ??= new TenantSettingRepository(_dbContext);
    public IUserRepository Users => _userRepository ??= new UserRepository(_dbContext);
    public IRoleRepository Roles => _roleRepository ??= new RoleRepository(_dbContext);
    public IPermissionRepository Permissions => _permissionRepository ??= new PermissionRepository(_dbContext);
    public IUserRoleRepository UserRoles => _userroleRepository ??= new UserRoleRepository(_dbContext);
    public IRolePermissionRepository RolePermissions => _rolepermissionRepository ??= new RolePermissionRepository(_dbContext);
    public IDepartmentRepository Departments => _departmentRepository ??= new DepartmentRepository(_dbContext);
    public ISkillRepository Skills => _skillRepository ??= new SkillRepository(_dbContext);
    public IJobRepository Jobs => _jobRepository ??= new JobRepository(_dbContext);
    public IJobSkillRepository JobSkills => _jobskillRepository ??= new JobSkillRepository(_dbContext);
    public ICandidateRepository Candidates => _candidateRepository ??= new CandidateRepository(_dbContext);
    public ICandidateSkillRepository CandidateSkills => _candidateskillRepository ??= new CandidateSkillRepository(_dbContext);
    public ICandidateExperienceRepository CandidateExperiences => _candidateexperienceRepository ??= new CandidateExperienceRepository(_dbContext);
    public ICandidateEducationRepository CandidateEducations => _candidateeducationRepository ??= new CandidateEducationRepository(_dbContext);
    public ICandidateDocumentRepository CandidateDocuments => _candidatedocumentRepository ??= new CandidateDocumentRepository(_dbContext);
    public IApplicationRepository Applications => _applicationRepository ??= new ApplicationRepository(_dbContext);
    public IPipelineRepository Pipelines => _pipelineRepository ??= new PipelineRepository(_dbContext);
    public IPipelineStageRepository PipelineStages => _pipelinestageRepository ??= new PipelineStageRepository(_dbContext);
    public IApplicationStageHistoryRepository ApplicationStageHistorys => _applicationstagehistoryRepository ??= new ApplicationStageHistoryRepository(_dbContext);
    public ICandidateActivityRepository CandidateActivitys => _candidateactivityRepository ??= new CandidateActivityRepository(_dbContext);
    public IInterviewRepository Interviews => _interviewRepository ??= new InterviewRepository(_dbContext);
    public IInterviewParticipantRepository InterviewParticipants => _interviewparticipantRepository ??= new InterviewParticipantRepository(_dbContext);
    public IInterviewCriteriaRepository InterviewCriterias => _interviewcriteriaRepository ??= new InterviewCriteriaRepository(_dbContext);
    public IInterviewFeedbackRepository InterviewFeedbacks => _interviewfeedbackRepository ??= new InterviewFeedbackRepository(_dbContext);
    public IInterviewScoreRepository InterviewScores => _interviewscoreRepository ??= new InterviewScoreRepository(_dbContext);
    public IAssessmentRepository Assessments => _assessmentRepository ??= new AssessmentRepository(_dbContext);
    public IQuestionRepository Questions => _questionRepository ??= new QuestionRepository(_dbContext);
    public IQuestionOptionRepository QuestionOptions => _questionoptionRepository ??= new QuestionOptionRepository(_dbContext);
    public IAssessmentAssignmentRepository AssessmentAssignments => _assessmentassignmentRepository ??= new AssessmentAssignmentRepository(_dbContext);
    public ICandidateAnswerRepository CandidateAnswers => _candidateanswerRepository ??= new CandidateAnswerRepository(_dbContext);
    public IAssessmentResultRepository AssessmentResults => _assessmentresultRepository ??= new AssessmentResultRepository(_dbContext);
    public INotificationRepository Notifications => _notificationRepository ??= new NotificationRepository(_dbContext);
    public INotificationTemplateRepository NotificationTemplates => _notificationtemplateRepository ??= new NotificationTemplateRepository(_dbContext);
    public IAuditLogRepository AuditLogs => _auditlogRepository ??= new AuditLogRepository(_dbContext);

    public async Task<int> CompleteAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
