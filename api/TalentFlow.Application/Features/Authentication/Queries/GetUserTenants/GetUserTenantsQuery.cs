using MediatR;
using System;
using System.Collections.Generic;

namespace TalentFlow.Application.Features.Authentication.Queries.GetUserTenants
{
    public class GetUserTenantsQuery : IRequest<List<TenantOptionDto>>
    {
        public Guid UserId { get; set; }
    }
}