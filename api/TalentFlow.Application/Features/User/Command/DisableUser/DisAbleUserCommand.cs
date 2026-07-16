using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.User.Command.DisableUser
{
    public class DisAbleUserCommand : IRequest<BaseCommandResponse>
    {
        public Guid Id { get; set; }

    }
}
