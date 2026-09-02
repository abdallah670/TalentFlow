using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Application.Features.Skills.Command.UpdateSkillCommand
{
    public class UpdateSkillCommandHandler : IRequestHandler<UpdateSkillCommand, BaseCommandResponse<bool>>
    {
        private readonly ILogger<UpdateSkillCommandHandler> logger;
        private readonly IUnitOfWork unitOfWork;

        public UpdateSkillCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateSkillCommandHandler> logger)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse<bool>> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
        {
         
            logger.LogInformation("Handling {Handler}", nameof(UpdateSkillCommandHandler));
            var skill = await unitOfWork.Skills.GetByIdAsync(request.id);

            if (skill is null)
            {
                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = "Skill not found."
                };
            }

            var duplicate = await unitOfWork.Skills.FindAsync(
       x => x.Name.ToLower() == request.name.ToLower() && x.Id != request.id);

            if (duplicate.Any())
            {
                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = "Another skill with this name already exists."
                };
            }
            skill.Name = request.name;

            await unitOfWork.Skills.UpdateAsync(skill);
            await unitOfWork.CompleteAsync();

            return new BaseCommandResponse<bool>
            {
                Success = true,
                Id = skill.Id
            };
        }
    }
}
