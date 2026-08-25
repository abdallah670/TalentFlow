using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Features.Job.DTOs;
using TalentFlow.Application.Features.Job.Queries.GetJobsCompany;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Models;
using TalentFlow.Domain.Enums;

namespace TalentFlow.Application.Features.Job.Queries.GetJobsForCandidate
{
    public class GetJobsForCandidateQueryHandler : IRequestHandler<GetJobsForCandidateQuery , PaginatedResult<DTOs.GetJobDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetJobsForCandidateQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetJobDto>> Handle(GetJobsForCandidateQuery request, CancellationToken cancellationToken)
        {
            var jobs = await unitOfWork.Jobs.GetAllAsync();
            jobs = jobs
                .Where(x => x.Status == JobStatus.Published)
                .ToList(); if (!string.IsNullOrWhiteSpace(request.Search))
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

          

            if (request.DepartmentId.HasValue)
            {
                jobs = jobs
                    .Where(x => x.DepartmentId == request.DepartmentId.Value)
                    .ToList();
            }
            var totalCount = jobs.Count();
            var pagedJobs = jobs
               .Skip((request.PageNumber - 1) * request.PageSize)
               .Take(request.PageSize)
               .ToList();

            var items = new List<GetJobDto>();
            foreach (var job in pagedJobs)
            {
                var tenant =
                    await unitOfWork.Tenants.GetByIdAsync(job.TenantId);

                items.Add(new GetJobDto
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
                });
            }
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
