using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.OfferModule
{
    public class Offer : BaseEntity, ITenantEntity
    {
        public Guid TenantId { get; set; }

        public Guid ApplicationId { get; set; }

        public decimal Salary { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string Status { get; set; } = default!;

        public string? Notes { get; set; }
    }
}
