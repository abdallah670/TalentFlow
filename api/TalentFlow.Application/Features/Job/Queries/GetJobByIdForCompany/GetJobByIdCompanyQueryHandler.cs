using MediatR;
using Microsoft.Extensions.Logging;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Features.Job.DTOs;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Job.Queries.GetJobById
{
    public class GetJobByIdCompanyQueryHandler : IRequestHandler<GetJObByIdCompanyQuery, BaseCommandResponse<GetJobDto>>
    {
        private readonly ILogger<GetJobByIdCompanyQueryHandler> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentTenantService currentTenantService;

        public GetJobByIdCompanyQueryHandler(IUnitOfWork unitOfWork, ICurrentTenantService currentTenantService, ILogger<GetJobByIdCompanyQueryHandler> logger)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.currentTenantService = currentTenantService;
        }

        public async Task<BaseCommandResponse<GetJobDto>> Handle(GetJObByIdCompanyQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(GetJobByIdCompanyQueryHandler));
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
            if (job.TenantId != currentTenantService.TenantId)
            {
                return new BaseCommandResponse<GetJobDto>
                {
                    Success = false,
                    Message = "This job does not belong to the current tenant."
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
                logger.LogError(ex, "Failed to get job by id (company) in {Handler}", nameof(GetJobByIdCompanyQueryHandler));
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
