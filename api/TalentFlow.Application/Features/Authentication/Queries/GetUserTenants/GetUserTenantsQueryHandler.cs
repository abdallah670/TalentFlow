using MediatR;
using TalentFlow.Application.Contracts.Persistence;

namespace TalentFlow.Application.Features.Authentication.Queries.GetUserTenants
{
    public class GetUserTenantsQueryHandler : IRequestHandler<GetUserTenantsQuery, List<TenantOptionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserTenantsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TenantOptionDto>> Handle(GetUserTenantsQuery request, CancellationToken cancellationToken)
        {
            var memberships = await _unitOfWork.UserTenants.FindAsync(
                x => x.UserId == request.UserId && x.IsActive);

            var result = new List<TenantOptionDto>();

            foreach (var m in memberships)
            {
                var tenant = await _unitOfWork.Tenants.GetByIdAsync(m.TenantId);
                result.Add(new TenantOptionDto
                {
                    TenantId = m.TenantId.ToString(),
                    TenantName = tenant?.Name ?? "",
                    Role = m.Role
                });
            }

            return result;
        }
    }
}