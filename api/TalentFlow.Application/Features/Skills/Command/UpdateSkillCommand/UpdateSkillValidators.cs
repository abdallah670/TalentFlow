using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Skills.Command.UpdateSkillCommand
{
    public class UpdateSkillValidators : AbstractValidator<UpdateSkillCommand>
    {
        public UpdateSkillValidators() {


            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Skill name is required.")
                .MaximumLength(100).WithMessage("Skill name must not exceed 100 characters.");
        }
    }
}
