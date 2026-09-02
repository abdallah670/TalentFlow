using MediatR;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Authentication.Commands.ResendVerification
{
    public class ResendVerificationCommand : IRequest<BaseCommandResponse<bool>>
    {
        public string Email { get; set; } = string.Empty;
    }
}