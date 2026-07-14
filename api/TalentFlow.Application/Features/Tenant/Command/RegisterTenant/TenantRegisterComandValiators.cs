using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Tenant.Command.RegisterTenant
{
    public class TenantRegisterComandValiators :AbstractValidator<TenantRegisterCommand>
    {
        public TenantRegisterComandValiators()
        {
            RuleFor(x => x.TentantName).NotEmpty().WithMessage("Tenant Name Is Required").MaximumLength(200);
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name Is Required").MaximumLength(200);
            RuleFor(x => x.LastName).NotEmpty().WithMessage("First Name Is Required").MaximumLength(200);
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email Is Required").EmailAddress().WithMessage("Invalid email format").MaximumLength(200);
            RuleFor(x => x.Password)
                            .NotEmpty().WithMessage("Password is required")
                            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                            .Matches("[0-9]").WithMessage("Password must contain at least one number");

        }
    }
}
