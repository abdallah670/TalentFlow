// TalentFlow.Application/Features/CandidateModule/Commands/UpdateSkills/UpdateSkillsHandler.cs
using MediatR;
using Microsoft.Extensions.Logging;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.CandidateModule;

namespace TalentFlow.Application.Features.CandidateModule.Commands.UpdateSkills
{
    public class UpdateSkillsHandler : IRequestHandler<UpdateSkillsCommand, UpdateSkillsResponse>
    {
        private readonly ILogger<UpdateSkillsHandler> logger;
        private readonly IcandidateProfileRepo candidateProfileRepo;

        public UpdateSkillsHandler(IcandidateProfileRepo candidateProfileRepo, ILogger<UpdateSkillsHandler> logger)
        {
            this.logger = logger;
            this.candidateProfileRepo = candidateProfileRepo;
        }

        public async Task<UpdateSkillsResponse> Handle(UpdateSkillsCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(UpdateSkillsHandler));

            var profile = await candidateProfileRepo.GetByUserIdWithSkillsAsync(request.UserId);

            if (profile is null)
            {
                profile = new CandidateProfile { UserId = request.UserId };
                await candidateProfileRepo.AddAsync(profile);
            }

            profile.Skills.Clear();

            foreach (var skillId in request.SkillIds.Distinct())
            {
                profile.Skills.Add(new CandidateProfileSkill
                {
                    SkillId = skillId
                });
            }
            profile.CoverLetter = request.CoverLetter;

            await candidateProfileRepo.UpdateAsync(profile);
            await candidateProfileRepo.SaveAsync(cancellationToken);

            return new UpdateSkillsResponse
            {
                Success = true,
                Message = "Skills updated successfully."
            };
        }
    }
}