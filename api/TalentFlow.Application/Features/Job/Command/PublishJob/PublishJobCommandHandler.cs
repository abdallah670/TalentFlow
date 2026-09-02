using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Job.Command.PublishJob
{
    public class publishJobCommandHandler : IRequestHandler<PublishJobCommand, BaseCommandResponse<bool>>
    {
        private readonly ILogger<publishJobCommandHandler> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentTenantService currentTenantService;

        public publishJobCommandHandler(IUnitOfWork unitOfWork, ICurrentTenantService currentTenantService, ILogger<publishJobCommandHandler> logger)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.currentTenantService = currentTenantService;
        }

        public async Task<BaseCommandResponse<bool>> Handle(PublishJobCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(publishJobCommandHandler));
            var job=await unitOfWork.Jobs.GetByIdAsync(request.JobId);
            if (job == null)
            {
                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = "Job not found",
                    Errors = new List<string> { "Job not found" }
                };
            }
           
            if (job.TenantId != currentTenantService.TenantId)
            {
                return new BaseCommandResponse<bool>
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
                return new BaseCommandResponse<bool>
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

            return new BaseCommandResponse<bool>
            {
                Id = job.Id,
                Success = true,
                Message = "Job published successfully",
            };

        }
    }
}
