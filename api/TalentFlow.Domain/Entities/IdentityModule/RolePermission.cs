using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.IdentityModule;

public class RolePermission : BaseEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}
