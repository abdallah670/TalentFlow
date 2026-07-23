using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Interfaces
{
    public interface IAuditService
    {
        Task CreateAuditLogsAsync(ChangeTracker changeTracker);
    }
}
