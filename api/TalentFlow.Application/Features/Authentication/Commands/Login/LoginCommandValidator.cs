using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Authontication.Commands.Login
{
    public class LoginCommandValidator :AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
