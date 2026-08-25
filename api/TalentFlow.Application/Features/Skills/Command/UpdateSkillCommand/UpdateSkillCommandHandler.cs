using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Application.Features.Skills.Command.UpdateSkillCommand
{
    public class UpdateSkillCommandHandler : IRequestHandler<UpdateSkillCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateSkillCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
        {
         
            var skill = await unitOfWork.Skills.GetByIdAsync(request.id);

            if (skill is null)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "Skill not found."
                };
            }

            var duplicate = await unitOfWork.Skills.FindAsync(
       x => x.Name.ToLower() == request.name.ToLower() && x.Id != request.id);

            if (duplicate.Any())
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "Another skill with this name already exists."
                };
            }
            skill.Name = request.name;

            await unitOfWork.Skills.UpdateAsync(skill);
            await unitOfWork.CompleteAsync();

            return new BaseCommandResponse
            {
                Success = true,
                Id = skill.Id
            };
        }
    }
}
