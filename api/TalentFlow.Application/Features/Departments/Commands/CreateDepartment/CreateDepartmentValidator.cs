using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentValidator :AbstractValidator<CreateDepartmentCommand>
    {
        public CreateDepartmentValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Department name is required")
                .MaximumLength(200);
        }
    }
}
