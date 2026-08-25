using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Job.Command.CloseJob
{
    public class CloseJobCommandHandler : IRequestHandler<CloseJobCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentTenantService currentTenantService;

        public CloseJobCommandHandler(IUnitOfWork unitOfWork, ICurrentTenantService currentTenantService)
        {
            this.unitOfWork = unitOfWork;
            this.currentTenantService = currentTenantService;
        }

        public async Task<BaseCommandResponse> Handle(CloseJobCommand request, CancellationToken cancellationToken)
        {
            var job = await unitOfWork.Jobs.GetByIdAsync(request.JobId);
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
            if (job.Status != Domain.Enums.JobStatus.Published)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "Only published jobs can be closed.",
                    Errors = new List<string>
        {
            "Job must be in Published status."
        }
                };
            }
            job.Status = Domain.Enums.JobStatus.Closed;
            await unitOfWork.Jobs.UpdateAsync(job);
            await unitOfWork.CompleteAsync();

            return new BaseCommandResponse
            {
                Id = job.Id,
                Success = true,
                Message = "Job closed successfully",
            };

        }
    }
}
