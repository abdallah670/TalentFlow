using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Application.Features.Skills.Command.CreateSkillCommand
{
    public class UpdateSkillComandHandler : IRequestHandler<CreateSkillCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateSkillComandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
        {
            var existing = await unitOfWork.Skills.FindAsync(x => x.Name.ToLower() == request.Name.ToLower());
            if (existing.Any())
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "Skill already exists."
                };
            }
            var skill =new Skill { Name = request.Name };
            await unitOfWork.Skills.AddAsync(skill);
            await unitOfWork.CompleteAsync();

            return new BaseCommandResponse
            {
                Success = true,
                Id = skill.Id
            };
        }
    }
}
