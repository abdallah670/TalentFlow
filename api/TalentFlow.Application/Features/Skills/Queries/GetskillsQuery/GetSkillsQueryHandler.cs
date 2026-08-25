using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;

namespace TalentFlow.Application.Features.Skills.Queries.GetskillsQuery
{
    public class GetSkillsQueryHandler : IRequestHandler<GetSkillsQuery, List<SkillDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetSkillsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<List<SkillDto>> Handle(GetSkillsQuery request, CancellationToken cancellationToken)
        {
            var skills = string.IsNullOrWhiteSpace(request.Search)
            ? await unitOfWork.Skills.GetAllAsync()
            : await unitOfWork.Skills.FindAsync(x => x.Name.ToLower().Contains(request.Search.ToLower()));

            var res = skills.Select(x=>new SkillDto
            {
                Id=x.Id,
                Name=x.Name,
            }).ToList();
            return res;

        }
    }
}
