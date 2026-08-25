using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Skills.Command.CreateSkillCommand
{
    public class CreateSkillValidators : AbstractValidator<CreateSkillCommand>
    {
        public CreateSkillValidators()
        {

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Skill name is required.")
                .MaximumLength(100).WithMessage("Skill name must not exceed 100 characters.");
        }
    }
}
