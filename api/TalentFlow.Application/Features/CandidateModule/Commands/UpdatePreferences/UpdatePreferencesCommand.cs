using MediatR;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Enums;

namespace TalentFlow.Application.Features.CandidateModule.Commands.UpdatePreferences
{
    public class UpdatePreferencesCommand : IRequest<BaseCommandResponse<bool>>
    {
        public Guid UserId { get; set; }
        public string? PreferredLocation { get; set; }
        public bool RemoteOnly { get; set; }
        public decimal? MinSalaryExpectation { get; set; }
        public decimal? MaxSalaryExpectation { get; set; }
        public string? Currency { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public WorkAuthorization? WorkAuthorization { get; set; }
    }
}