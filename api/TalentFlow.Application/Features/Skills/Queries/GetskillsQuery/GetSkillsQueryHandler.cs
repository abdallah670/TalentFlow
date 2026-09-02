using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Skills.Queries.GetskillsQuery
{
    public class GetSkillsQueryHandler : IRequestHandler<GetSkillsQuery, BaseCommandResponse<List<SkillDto>>>
    {
        private readonly ILogger<GetSkillsQueryHandler> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetSkillsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetSkillsQueryHandler> logger)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<BaseCommandResponse<List<SkillDto>>> Handle(GetSkillsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(GetSkillsQueryHandler));
            try
            {
            var skills = string.IsNullOrWhiteSpace(request.Search)
            ? await unitOfWork.Skills.GetAllAsync()
            : await unitOfWork.Skills.FindAsync(x => x.Name.ToLower().Contains(request.Search.ToLower()));

            var res = skills.Select(x=>new SkillDto
            {
                Id=x.Id,
                Name=x.Name,
            }).ToList();
            return new BaseCommandResponse<List<SkillDto>>
            {
                Success = true,
                Data = res
            };
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Failed to get skills in {Handler}", nameof(GetSkillsQueryHandler));
                return new BaseCommandResponse<List<SkillDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve skills.",
                    Errors = { ex.Message }
                };
            }
        }
    }
}
