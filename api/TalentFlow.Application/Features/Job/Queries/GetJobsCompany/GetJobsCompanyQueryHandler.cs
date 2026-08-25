using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Features.Job.DTOs;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Models;

namespace TalentFlow.Application.Features.Job.Queries.GetJobsCompany
{
    public class GetJobsCompanyQueryHandler : IRequestHandler<GetJobsCompanyQuery, PaginatedResult<DTOs.GetJobDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentTenantService tenantService;

        public GetJobsCompanyQueryHandler(IUnitOfWork unitOfWork, ICurrentTenantService tenantService)
        {
            this.unitOfWork = unitOfWork;
            this.tenantService = tenantService;
        }

        public async Task<PaginatedResult<GetJobDto>> Handle(GetJobsCompanyQuery request, CancellationToken cancellationToken)
        {
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
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            return new PaginatedResult<GetJobDto>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

        }
    }
}
