using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Interfaces
{
    public interface ICurrentTenantService
    {
        Guid TenantId { get; }
    }
}
