using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Authentication.Commands.changePassword
{
    public class ChangePasswordComand :IRequest<BaseCommandResponse>
    {
        public string OLdPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
    }
}
