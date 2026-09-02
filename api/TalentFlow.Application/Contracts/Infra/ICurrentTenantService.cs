using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Contracts.Infra  
{
    public interface ICurrentTenantService
    {
        Guid TenantId { get; }
    }
}
