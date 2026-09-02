using MediatR;
using System;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Authontication.Commands.SelectTenant
{
    public class SelectTenantCommand : IRequest<BaseCommandResponse<AuthResponse>>
    {
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
    }
}