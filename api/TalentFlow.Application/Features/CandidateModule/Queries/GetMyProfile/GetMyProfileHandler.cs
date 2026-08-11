// TalentFlow.Application/Features/CandidateModule/Queries/GetMyProfile/GetMyProfileHandler.cs
using MediatR;
using TalentFlow.Application.Contracts.Persistence;

namespace TalentFlow.Application.Features.CandidateModule.Queries.GetMyProfile
{
    public class GetMyProfileHandler : IRequestHandler<GetMyProfileQuery, GetMyProfileResponse?>
    {
        private readonly IcandidateProfileRepo candidateProfileRepo;

        public GetMyProfileHandler(IcandidateProfileRepo candidateProfileRepo)
        {
            this.candidateProfileRepo = candidateProfileRepo;
        }

        public async Task<GetMyProfileResponse?> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            var profile = await candidateProfileRepo
     .GetByUserIdWithSkillsAsync(request.UserId);

            if (profile is null)
                return null;

            bool professional =
       !string.IsNullOrWhiteSpace(profile.PhoneNumber)
       && !string.IsNullOrWhiteSpace(profile.CurrentJobTitle)
       && profile.TotalYearsOfExperience.HasValue;
            bool resume =
                !string.IsNullOrWhiteSpace(profile.ResumeUrl);

            bool skills =
                profile.Skills.Any();

            bool preferences =
                !string.IsNullOrWhiteSpace(profile.PreferredLocation);

            int currentStep = 2;

            if (!professional)
                currentStep = 2;
            else if (!resume || !skills)
                currentStep = 3;
            else if (!preferences)
                currentStep = 4;
            else
                currentStep = 5;
            return new GetMyProfileResponse
            {
                PhoneNumber = profile.PhoneNumber,
                CurrentJobTitle = profile.CurrentJobTitle,
                CurrentCompany = profile.CurrentCompany,
                TotalYearsOfExperience = profile.TotalYearsOfExperience,
                LinkedInUrl = profile.LinkedInUrl,
                PortfolioUrl = profile.PortfolioUrl,
                ResumeUrl = profile.ResumeUrl,
                ResumeFileName = profile.ResumeFileName,
                PreferredLocation = profile.PreferredLocation,
                RemoteOnly = profile.RemoteOnly,
                MinSalaryExpectation = profile.MinSalaryExpectation,
                MaxSalaryExpectation = profile.MaxSalaryExpectation,
                Currency = profile.Currency,
                AvailableFrom = profile.AvailableFrom,
                WorkAuthorization = profile.WorkAuthorization,
                ProfessionalProfileCompleted = professional,
                ResumeUploaded = resume,
                SkillsCompleted = skills,
                PreferencesCompleted = preferences,
                CurrentStep = currentStep,

            };
        }
    }
}