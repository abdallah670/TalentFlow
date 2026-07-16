using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Authentication.Commands.ResetPasswors
{
    public class ResetPasswordCommand :IRequest<BaseCommandResponse>
    {
        public string Email { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
    }
}
