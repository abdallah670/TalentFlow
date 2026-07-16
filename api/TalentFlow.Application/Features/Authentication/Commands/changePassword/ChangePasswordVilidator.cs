using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Authentication.Commands.changePassword
{
    public class ChangePasswordVilidator : AbstractValidator<ChangePasswordComand>
    {
        public ChangePasswordVilidator()
        {
            {
                RuleFor(x => x.OLdPassword)
    .NotEmpty()
    .WithMessage("Current password is required.");

                RuleFor(x => x.NewPassword)
                        .NotEmpty().WithMessage("Password is required")
                        .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                        .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                        .Matches("[0-9]").WithMessage("Password must contain at least one number");


                RuleFor(x => x.ConfirmPassword)
                    .NotEmpty()
                            .Equal(x => x.NewPassword).WithMessage("Passwords do not match");
            }

        }
    }
}
