using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.WorkflowModule;

public class Pipeline : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }

    public string Name { get; set; } = default!;
}
