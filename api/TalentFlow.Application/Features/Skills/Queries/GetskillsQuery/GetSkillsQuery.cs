using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Skills.Queries.GetskillsQuery
{
    public class GetSkillsQuery :IRequest<List<SkillDto>>
    {
        public string? Search {  get; set; }

    }
}
