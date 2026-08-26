using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Features.Job.DTOs;
using TalentFlow.Application.Interfaces;

namespace TalentFlow.Application.Features.Job.Queries.GetJobById
{
    public class GetJobByIdCandidateQueryHandler : IRequestHandler<GetJObByIdCandiateQuery, GetJobDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentTenantService currentTenantService;

        public GetJobByIdCandidateQueryHandler(IUnitOfWork unitOfWork, ICurrentTenantService currentTenantService)
        {
            this.unitOfWork = unitOfWork;
            this.currentTenantService = currentTenantService;
        }

        public async Task<GetJobDto> Handle(GetJObByIdCandiateQuery request, CancellationToken cancellationToken)
        {
            var job = await unitOfWork.Jobs.GetByIdAsync(request.JobId);
            if (job == null)
            {
                throw new Exception($"Job with ID {request.JobId} not found.");

            }
            if (job.Status != Domain.Enums.JobStatus.Published)
            {
                throw new Exception("This job is not available.");
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

            return jobDto;
        }
    }
}
