using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Tenant.Queries.GetCurrentTenant
{
    public class GetCurrentTenantQuery: IRequest<BaseCommandResponse>
    {
    }
}
