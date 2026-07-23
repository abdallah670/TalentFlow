using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Roles.Command.AssignPermissions
{
    public class AssignPermissionsCommand : IRequest<BaseCommandResponse>
    {
        public Guid RoleId { get; set; }

        public List<Guid> PermissionIds { get; set; } = new();
    }
}
