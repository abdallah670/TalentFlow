using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Authentication.Commands.Logout
{
    public class LogoutCommand :IRequest<bool>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
