using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Enums;

namespace TalentFlow.Application.Features.Job.Command.CreateJob
{
    public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, BaseCommandResponse<bool>>
    {
        private readonly ILogger<CreateJobCommandHandler> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly ICurrentTenantService currentTenantService;

        public CreateJobCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ICurrentTenantService currentTenantService, ILogger<CreateJobCommandHandler> logger)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.currentTenantService = currentTenantService;
        }

        public async Task<BaseCommandResponse<bool>> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(CreateJobCommandHandler));
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

            return new BaseCommandResponse<bool>
            {
                Success = true,
                Message = "Job created successfully.",
                Id = job.Id
            };
        }
    }
}
