using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Job.Command.UpdateJob
{
    public class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly ICurrentTenantService currentTenantService;

        public UpdateJobCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ICurrentTenantService currentTenantService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.currentTenantService = currentTenantService;
        }

        public async Task<BaseCommandResponse> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
        {
            var job = await unitOfWork.Jobs.GetByIdAsync(request.JobId);
            if (job == null)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "Job not found.",
                    Errors = new List<string> { "Job not found." }
                };

            }
            if (job.TenantId != currentTenantService.TenantId)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "You are not authorized to update this job.",
                    Errors = new List<string>
                    {
                        "This job does not belong to the current tenant."
                    }
                };
            }
            job.DepartmentId = request.DepartmentId;
            job.Title = request.Title;
            job.Description = request.Description;
            job.EmploymentType = request.EmploymentType;
            job.ExperienceLevel = request.ExperienceLevel;
            job.SalaryMin = request.SalaryMin;
            job.SalaryMax = request.SalaryMax;
            job.OpenDate = request.OpenDate;
            job.CloseDate = request.CloseDate;
            await unitOfWork.Jobs.UpdateAsync(job);
            await unitOfWork.CompleteAsync();
            return new BaseCommandResponse
            {
                Success = true,
                Message = "Job updated successfully."
            };


        }
    }
}
