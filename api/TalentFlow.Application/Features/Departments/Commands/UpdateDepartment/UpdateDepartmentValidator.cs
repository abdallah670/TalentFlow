using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentValidator :AbstractValidator<UpdateDepartmentCommand>
    {
        public UpdateDepartmentValidator() 
        {
            RuleFor(x => x.id).NotEmpty();
            RuleFor(x => x.name).NotEmpty().WithMessage("Department name is required")
               .MaximumLength(200);
        }
    }
}
