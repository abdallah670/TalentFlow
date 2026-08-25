using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Job.Command.PublishJob
{
    public class publishJobCommandHandler : IRequestHandler<PublishJobCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentTenantService currentTenantService;

        public publishJobCommandHandler(IUnitOfWork unitOfWork, ICurrentTenantService currentTenantService)
        {
            this.unitOfWork = unitOfWork;
            this.currentTenantService = currentTenantService;
        }

        public async Task<BaseCommandResponse> Handle(PublishJobCommand request, CancellationToken cancellationToken)
        {
            var job=await unitOfWork.Jobs.GetByIdAsync(request.JobId);
            if (job == null)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "Job not found",
                    Errors = new List<string> { "Job not found" }
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
            if (job.Status != Domain.Enums.JobStatus.Draft)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "Only draft jobs can be published.",
                    Errors = new List<string>
        {
            "Job must be in Draft status."
        }
                };
            }
            job.Status = Domain.Enums.JobStatus.Published;
            await unitOfWork.Jobs.UpdateAsync(job);
            await unitOfWork.CompleteAsync();

            return new BaseCommandResponse
            {
                Id = job.Id,
                Success = true,
                Message = "Job published successfully",
            };

        }
    }
}
