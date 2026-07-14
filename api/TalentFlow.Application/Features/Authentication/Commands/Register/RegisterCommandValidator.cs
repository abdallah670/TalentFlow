using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Authontication.Commands.Register
{
    public class RegisterCommandValidator :AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name Is Required")
                .MaximumLength(100);
            RuleFor(x => x.LastName)
         .NotEmpty().WithMessage("Last name is required")
         .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number");

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company name is required")
                .MaximumLength(200);
        }

    }
}
