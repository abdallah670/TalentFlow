using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.OfferModule
{
    public class OfferApproval : BaseEntity, ITenantEntity
    {
        public Guid TenantId { get; set; }

        public Guid OfferId { get; set; }

        public Guid ApproverId { get; set; }

        public bool Approved { get; set; }

        public string? Comment { get; set; }

        public DateTime? ApprovedAt { get; set; }
    }
}
