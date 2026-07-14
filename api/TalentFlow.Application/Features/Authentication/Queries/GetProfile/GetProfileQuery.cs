using MediatR;

namespace TalentFlow.Application.Features.Authentication.Queries.GetProfile
{
    public class GetProfileQuery : IRequest<UserProfileDto>
    {
    }
}