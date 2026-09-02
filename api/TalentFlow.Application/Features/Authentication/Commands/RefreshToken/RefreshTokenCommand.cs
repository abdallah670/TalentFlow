using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Authontication.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<BaseCommandResponse<AuthResponse>>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
