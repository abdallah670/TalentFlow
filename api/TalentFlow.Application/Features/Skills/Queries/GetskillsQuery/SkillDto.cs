using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Skills.Queries.GetskillsQuery
{
    public class SkillDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
