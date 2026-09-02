using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Application.Features.Skills.Command.CreateSkillCommand
{
    public class UpdateSkillComandHandler : IRequestHandler<CreateSkillCommand, BaseCommandResponse<bool>>
    {
        private readonly ILogger<UpdateSkillComandHandler> logger;
        private readonly IUnitOfWork unitOfWork;

        public UpdateSkillComandHandler(IUnitOfWork unitOfWork, ILogger<UpdateSkillComandHandler> logger)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse<bool>> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(UpdateSkillComandHandler));
            var existing = await unitOfWork.Skills.FindAsync(x => x.Name.ToLower() == request.Name.ToLower());
            if (existing.Any())
            {
                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = "Skill already exists."
                };
            }
            var skill =new Skill { Name = request.Name };
            await unitOfWork.Skills.AddAsync(skill);
            await unitOfWork.CompleteAsync();

            return new BaseCommandResponse<bool>
            {
                Success = true,
                Id = skill.Id
            };
        }
    }
}
