using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Authontication.Commands.RefreshToken
{
    public class RefreshTokenCommand:IRequest<AuthResponse>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
