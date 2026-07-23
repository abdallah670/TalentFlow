using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Interfaces;

namespace TalentFlow.Infrastructure.Service
{
    public class CurrentTenantService : ICurrentTenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentTenantService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid TenantId
        {
            get
            {
                var tenantId = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst("TenantId")?
                    .Value;

                return Guid.Parse(tenantId!);
            }
        }
    }
}
