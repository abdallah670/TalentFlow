using MediatR;

namespace TalentFlow.Application.Features.CandidateModule.Commands.UpdateSkills
{
    public class UpdateSkillsCommand : IRequest<UpdateSkillsResponse>
    {
        public Guid UserId { get; set; }
        public List<Guid> SkillIds { get; set; } = new();
    }

    public class UpdateSkillsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = default!;
    }
}