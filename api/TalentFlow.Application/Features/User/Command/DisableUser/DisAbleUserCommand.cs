using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Features.User.Query.GetUser;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.User.Command.DisableUser
{
    public class DisAbleUserCommand : IRequest<BaseCommandResponse<GetUserDTOs>>
    {
        public Guid Id { get; set; }

    }
}
