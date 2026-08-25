using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Skills.Command.UpdateSkillCommand
{
    public class UpdateSkillCommand : IRequest<BaseCommandResponse>
    {
        public Guid id { get; set; }
        public string name { get; set; }=string.Empty;
    }
}
