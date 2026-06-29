using System;
using Microsoft.AspNetCore.Identity;

namespace TalentFlow.Domain.Entities.IdentityModule;

public class Role : IdentityRole<Guid>
{
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
