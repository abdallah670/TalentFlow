using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Roles.Command.CreateRole
{
    public class CreateRoleCommand :IRequest <BaseCommandResponse>
    {
        public string Name { get; set; } = default!;

    }
}
