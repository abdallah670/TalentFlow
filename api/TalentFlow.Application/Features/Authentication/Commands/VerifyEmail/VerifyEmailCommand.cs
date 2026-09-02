using MediatR;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Authentication.Commands.VerifyEmail
{
    public class VerifyEmailCommand : IRequest<BaseCommandResponse<bool>>
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}