using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Authentication.Commands.ForgetPassword
{
    public class ForgetPasswordComand :IRequest<BaseCommandResponse>
    {
        public string Email { get; set; } = default!;
    }
}
