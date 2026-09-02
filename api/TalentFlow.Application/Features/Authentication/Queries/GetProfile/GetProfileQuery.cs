using MediatR;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Authentication.Queries.GetProfile
{
    public class GetProfileQuery : IRequest<BaseCommandResponse<UserProfileDto>>
    {
    }
}