using MediatR;
using System;
using System.Collections.Generic;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Skills.Queries.GetskillsQuery
{
    public class GetSkillsQuery : IRequest<BaseCommandResponse<List<SkillDto>>>
    {
        public string? Search {  get; set; }

    }
}
