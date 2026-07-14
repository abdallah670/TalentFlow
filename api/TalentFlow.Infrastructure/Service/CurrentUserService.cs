using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TalentFlow.Application.Interfaces;

namespace TalentFlow.Infrastructure.Service
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?
    .User?
    .FindFirstValue(ClaimTypes.NameIdentifier);

                return Guid.TryParse(userId, out var id)
                    ? id
                    : Guid.Empty;
            }
        }
        public Guid TenantId
        {
            get
            {
                var tenantId = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirstValue("tenantId");

                return Guid.TryParse(tenantId, out var id)
                    ? id
                    : Guid.Empty;
            }
        }
    }
}