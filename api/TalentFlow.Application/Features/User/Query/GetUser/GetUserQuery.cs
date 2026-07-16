using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.User.Query.GetUser
{
    public class GetUserQuery :IRequest<BaseCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
