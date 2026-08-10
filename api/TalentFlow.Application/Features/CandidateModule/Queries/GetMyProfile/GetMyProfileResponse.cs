using TalentFlow.Domain.Enums;

namespace TalentFlow.Application.Features.CandidateModule.Queries.GetMyProfile
{
    public class GetMyProfileResponse
    {
        public string? PhoneNumber { get; set; }
        public string? CurrentJobTitle { get; set; }
        public string? CurrentCompany { get; set; }
        public int? TotalYearsOfExperience { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? PortfolioUrl { get; set; }
        public string? ResumeUrl { get; set; }
        public string? ResumeFileName { get; set; }
        public string? PreferredLocation { get; set; }
        public bool RemoteOnly { get; set; }
        public decimal? MinSalaryExpectation { get; set; }
        public decimal? MaxSalaryExpectation { get; set; }
        public string? Currency { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public WorkAuthorization? WorkAuthorization { get; set; }
        public bool ProfessionalProfileCompleted { get; set; }

        public bool ResumeUploaded { get; set; }

        public bool SkillsCompleted { get; set; }

        public bool PreferencesCompleted { get; set; }

        public int CurrentStep { get; set; }
    }
}