using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.RecruitmentModule;

public class Job : BaseEntity, ITenantEntity, ISoftDelete
{
    public Guid TenantId { get; set; }

    public Guid DepartmentId { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string EmploymentType { get; set; } = default!;
    public string ExperienceLevel { get; set; } = default!;
    public decimal SalaryMin { get; set; }
    public decimal SalaryMax { get; set; }
    public string Status { get; set; } = default!;
    public DateTime OpenDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public Guid CreatedByUserId { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}
