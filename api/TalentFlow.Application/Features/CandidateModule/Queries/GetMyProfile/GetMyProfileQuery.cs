using MediatR;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.CandidateModule.Queries.GetMyProfile
{
    public class GetMyProfileQuery : IRequest<BaseCommandResponse<GetMyProfileResponse>>
    {
        public Guid UserId { get; set; }
    }
}