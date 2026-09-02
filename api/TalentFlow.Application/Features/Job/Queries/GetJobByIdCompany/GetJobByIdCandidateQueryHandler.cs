using MediatR;
using Microsoft.Extensions.Logging;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Features.Job.DTOs;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Job.Queries.GetJobById
{
    public class GetJobByIdCandidateQueryHandler : IRequestHandler<GetJObByIdCandiateQuery, BaseCommandResponse<GetJobDto>>
    {
        private readonly ILogger<GetJobByIdCandidateQueryHandler> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentTenantService currentTenantService;

        public GetJobByIdCandidateQueryHandler(IUnitOfWork unitOfWork, ICurrentTenantService currentTenantService, ILogger<GetJobByIdCandidateQueryHandler> logger)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.currentTenantService = currentTenantService;
        }

        public async Task<BaseCommandResponse<GetJobDto>> Handle(GetJObByIdCandiateQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(GetJobByIdCandidateQueryHandler));
            try
            {
            var job = await unitOfWork.Jobs.GetByIdAsync(request.JobId);
            if (job == null)
            {
                return new BaseCommandResponse<GetJobDto>
                {
                    Success = false,
                    Message = $"Job with ID {request.JobId} not found."
                };
            }
            if (job.Status != Domain.Enums.JobStatus.Published)
            {
                return new BaseCommandResponse<GetJobDto>
                {
                    Success = false,
                    Message = "This job is not available."
                };
            }
            var tenant = await unitOfWork.Tenants.GetByIdAsync(job.TenantId);


            var jobDto = new GetJobDto
            {
                Id = job.Id,
                DepartmentId = job.DepartmentId,
                Title = job.Title,
                Description = job.Description,
                EmploymentType = job.EmploymentType,
                ExperienceLevel = job.ExperienceLevel,
                SalaryMin = job.SalaryMin,
                SalaryMax = job.SalaryMax,
                CompanyName = tenant?.Name ?? "Unknown",
                Status = job.Status,
                OpenDate = job.OpenDate,
                CloseDate = job.CloseDate
            };

            return new BaseCommandResponse<GetJobDto>
            {
                Success = true,
                Data = jobDto
            };
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Failed to get job by id (candidate) in {Handler}", nameof(GetJobByIdCandidateQueryHandler));
                return new BaseCommandResponse<GetJobDto>
                {
                    Success = false,
                    Message = "Failed to retrieve job.",
                    Errors = { ex.Message }
                };
            }
        }
    }
}
