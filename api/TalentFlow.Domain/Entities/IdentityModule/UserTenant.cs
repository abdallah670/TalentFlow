using System;
using TalentFlow.Domain.Common;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Domain.Entities.TenantModule
{
    public class UserTenant : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; } = default!;

        public string Role { get; set; } = default!;
    }
}