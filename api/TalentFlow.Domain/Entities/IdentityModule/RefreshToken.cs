using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Domain.Entities.IdentityModule
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public Guid UserId { get; set; } = default!;

        public User User { get; set; } = default!;

        public string TokenHash { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool IsActive => RevokedAt == null && !IsExpired;
    }
}
