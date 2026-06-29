using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.IdentityModule;

public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}
