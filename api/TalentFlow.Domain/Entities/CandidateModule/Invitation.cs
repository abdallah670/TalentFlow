using TalentFlow.Domain.Common;
using TalentFlow.Domain.Enums;
namespace TalentFlow.Domain.Entities.CandidateModule
{
    public class Invitation : BaseEntity
    {
        public Guid TenantId { get; set; }

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public Roles Role { get; set; }

        public string Token { get; set; } = default!;

        public DateTime ExpirationDate { get; set; }

        public bool IsUsed { get; set; }
        public bool IsAccepted { get; set; } = false;

        public Guid InvitedByUserId { get; set; }

        public string? CustomMessage { get; set; }
    }
}