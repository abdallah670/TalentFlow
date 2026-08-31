using MediatR;
using System;

namespace TalentFlow.Application.Features.Authontication.Commands.SelectTenant
{
    public class SelectTenantCommand : IRequest<AuthResponse>
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
    }
}