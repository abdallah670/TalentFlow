using FluentValidation;

namespace TalentFlow.Application.Features.CandidateModule.Commands.UpdateSkills
{
    public class UpdateSkillsValidator : AbstractValidator<UpdateSkillsCommand>
    {
        public UpdateSkillsValidator()
        {
            RuleFor(x => x.SkillIds)
                .NotNull();

            RuleForEach(x => x.SkillIds)
                .NotEmpty();
        }
    }
}