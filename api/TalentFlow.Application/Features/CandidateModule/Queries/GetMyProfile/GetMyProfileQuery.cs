using MediatR;

namespace TalentFlow.Application.Features.CandidateModule.Queries.GetMyProfile
{
    public class GetMyProfileQuery : IRequest<GetMyProfileResponse?>
    {
        public Guid UserId { get; set; }
    }
}