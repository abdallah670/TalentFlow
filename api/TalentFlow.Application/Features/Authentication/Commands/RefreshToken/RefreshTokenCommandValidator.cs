using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Features.Authontication.Commands.Login;
using TalentFlow.Application.Features.Authontication.Commands.RefreshToken;

namespace TalentFlow.Application.Features.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandValidator :AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty();
        }
    }
}
