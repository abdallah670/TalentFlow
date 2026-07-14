using System;
using Microsoft.AspNetCore.Identity;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.IdentityModule;

public class User : IdentityUser<Guid>, ITenantEntity
{
    public Guid TenantId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }
    = new List<RefreshToken>();
}
