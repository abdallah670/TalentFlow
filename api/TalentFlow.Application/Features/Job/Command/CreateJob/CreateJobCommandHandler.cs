using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Enums;

namespace TalentFlow.Application.Features.Job.Command.CreateJob
{
    public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly ICurrentTenantService currentTenantService;

        public CreateJobCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ICurrentTenantService currentTenantService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.currentTenantService = currentTenantService;
        }

        public async Task<BaseCommandResponse> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            var job = new Domain.Entities.RecruitmentModule.Job
            {
                TenantId = currentTenantService.TenantId,
                DepartmentId = request.DepartmentId,
                Title = request.Title,
                Description = request.Description,
                EmploymentType = request.EmploymentType,
                ExperienceLevel = request.ExperienceLevel,
                SalaryMin = request.SalaryMin,
                SalaryMax = request.SalaryMax,
                Status =JobStatus.Draft,
                OpenDate = request.OpenDate,
                CloseDate = request.CloseDate,
                CreatedByUserId = currentUserService.UserId,
            };

           await unitOfWork.Jobs.AddAsync(job);
            await unitOfWork.CompleteAsync();

            return new BaseCommandResponse
            {
                Success = true,
                Message = "Job created successfully.",
                Id = job.Id
            };
        }
    }
}
