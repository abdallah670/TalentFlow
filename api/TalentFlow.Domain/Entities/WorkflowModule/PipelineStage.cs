using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.WorkflowModule;

public class PipelineStage : BaseEntity
{
    public Guid PipelineId { get; set; }
    public string Name { get; set; } = default!;
    public int StageOrder { get; set; }
}
