using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Authontication.Commands.Login
{
    public class LoginCommand : IRequest<BaseCommandResponse<AuthResponse>>
    {
        public string Email { get; set; }=string.Empty;
        public string Password { get; set; }=string.Empty;
    }
}
