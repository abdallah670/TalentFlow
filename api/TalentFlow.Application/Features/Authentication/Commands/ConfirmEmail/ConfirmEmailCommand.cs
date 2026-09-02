using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Authentication.Commands.ConfermEmail
{
    public class ConfirmEmailCommand :IRequest<BaseCommandResponse<bool>>
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
