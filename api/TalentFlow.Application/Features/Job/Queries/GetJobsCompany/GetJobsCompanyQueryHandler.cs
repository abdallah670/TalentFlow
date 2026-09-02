using MediatR;
using Microsoft.Extensions.Logging;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Features.Job.DTOs;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Models;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Job.Queries.GetJobsCompany
{
    public class GetJobsCompanyQueryHandler : IRequestHandler<GetJobsCompanyQuery, BaseCommandResponse<PaginatedResult<GetJobDto>>>
    {
        private readonly ILogger<GetJobsCompanyQueryHandler> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentTenantService tenantService;

        public GetJobsCompanyQueryHandler(IUnitOfWork unitOfWork, ICurrentTenantService tenantService, ILogger<GetJobsCompanyQueryHandler> logger)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.tenantService = tenantService;
        }

        public async Task<BaseCommandResponse<PaginatedResult<GetJobDto>>> Handle(GetJobsCompanyQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(GetJobsCompanyQueryHandler));
            var jobs = await unitOfWork.Jobs.GetAllAsync();
            jobs=jobs.Where(x => x.TenantId == tenantService.TenantId).ToList();
            if(!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();
                jobs = jobs.Where(x => x.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                x.Description.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            }
            if (request.EmploymentType.HasValue)
            {
                jobs = jobs
                    .Where(x => x.EmploymentType == request.EmploymentType.Value)
                    .ToList();
            }

            if (request.ExperienceLevel.HasValue)
            {
                jobs = jobs
                    .Where(x => x.ExperienceLevel == request.ExperienceLevel.Value)
                    .ToList();
            }

            if (request.Status.HasValue)
            {
                jobs = jobs
                    .Where(x => x.Status == request.Status.Value)
                    .ToList();
            }

            if (request.DepartmentId.HasValue)
            {
                jobs = jobs
                    .Where(x => x.DepartmentId == request.DepartmentId.Value)
                    .ToList();
            }
            var tenant = await unitOfWork.Tenants.GetByIdAsync(tenantService.TenantId);
            var totalCount = jobs.Count();
            var items = jobs
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new GetJobDto
                {
                    Id = x.Id,
                    DepartmentId = x.DepartmentId,
                    Title = x.Title,
                    CompanyName = tenant.Name,
                    Description = x.Description,
                    EmploymentType = x.EmploymentType,
                    ExperienceLevel = x.ExperienceLevel,
                    SalaryMin = x.SalaryMin,
                    SalaryMax = x.SalaryMax,
                    Status = x.Status,
                    OpenDate = x.OpenDate,
                    CloseDate = x.CloseDate
                }).ToList();
            return new BaseCommandResponse<PaginatedResult<GetJobDto>>
            {
                Success = true,
                Data = new PaginatedResult<GetJobDto>
                {
                Items = items,
                Page = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                }
            };

        }
    }
}
