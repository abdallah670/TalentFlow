using MediatR;
using System;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.User.Command.UpdateUserRole
{
    public class UpdateUserRoleCommand : IRequest<BaseCommandResponse<bool>>
    {
        public Guid Id { get; set; }
        public string Role { get; set; } = default!;
    }
}