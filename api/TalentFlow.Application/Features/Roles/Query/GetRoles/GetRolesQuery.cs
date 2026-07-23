using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Roles.Query.GetRoles
{
    public class GetRolesQuery :IRequest <BaseCommandResponse>
    {
    }
}
