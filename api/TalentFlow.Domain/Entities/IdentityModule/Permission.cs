using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.IdentityModule;

public class Permission : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}
