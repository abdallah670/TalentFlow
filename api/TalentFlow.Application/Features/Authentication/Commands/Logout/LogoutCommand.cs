using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Authentication.Commands.Logout
{
    public class LogoutCommand : IRequest<BaseCommandResponse<bool>>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
