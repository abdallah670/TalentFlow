using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Roles.Command.CreateRole
{
    public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleValidator() 
        {

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
        }
    }
}
