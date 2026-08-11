// TalentFlow.Application/Features/CandidateModule/Commands/UpdatePreferences/UpdatePreferencesHandler.cs
using MediatR;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.CandidateModule;

namespace TalentFlow.Application.Features.CandidateModule.Commands.UpdatePreferences
{
    public class UpdatePreferencesHandler : IRequestHandler<UpdatePreferencesCommand, UpdatePreferencesResponse>
    {
        private readonly IcandidateProfileRepo candidateProfileRepo;

        public UpdatePreferencesHandler(IcandidateProfileRepo candidateProfileRepo)
        {
            this.candidateProfileRepo = candidateProfileRepo;
        }

        public async Task<UpdatePreferencesResponse> Handle(UpdatePreferencesCommand request, CancellationToken cancellationToken)
        {
            var existingProfiles = await candidateProfileRepo.FindAsync(x => x.UserId == request.UserId);
            var profile = existingProfiles.FirstOrDefault();

            if (profile is null)
            {
                profile = new CandidateProfile
                {
                    UserId = request.UserId,
                    PreferredLocation = request.PreferredLocation,
                    RemoteOnly = request.RemoteOnly,
                    MinSalaryExpectation = request.MinSalaryExpectation,
                    MaxSalaryExpectation = request.MaxSalaryExpectation,
                    Currency = request.Currency,
                    AvailableFrom = request.AvailableFrom,
                    WorkAuthorization = request.WorkAuthorization
                };

                await candidateProfileRepo.AddAsync(profile);
            }
            else
            {
                profile.PreferredLocation = request.PreferredLocation;
                profile.RemoteOnly = request.RemoteOnly;
                profile.MinSalaryExpectation = request.MinSalaryExpectation;
                profile.MaxSalaryExpectation = request.MaxSalaryExpectation;
                profile.Currency = request.Currency;
                profile.AvailableFrom = request.AvailableFrom;
                profile.WorkAuthorization = request.WorkAuthorization;

                await candidateProfileRepo.UpdateAsync(profile);
            }

            await candidateProfileRepo.SaveAsync(cancellationToken);

            return new UpdatePreferencesResponse
            {
                Success = true,
                Message = "Preferences updated successfully."
            };
        }
    }
}