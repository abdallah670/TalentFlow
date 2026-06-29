using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.RecruitmentModule;

public class Candidate : BaseEntity, ITenantEntity, ISoftDelete
{
    public Guid TenantId { get; set; }

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? PortfolioUrl { get; set; }

    public string? GithubUrl { get; set; }
    public string? CurrentPosition { get; set; }
    public int? YearsOfExperience { get; set; }
    public string? Source { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}