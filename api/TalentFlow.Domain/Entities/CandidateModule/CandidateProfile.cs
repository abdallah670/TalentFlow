// TalentFlow.Domain/Entities/CandidateModule/CandidateProfile.cs
using TalentFlow.Domain.Common;
using TalentFlow.Domain.Entities.IdentityModule;
using TalentFlow.Domain.Enums;

namespace TalentFlow.Domain.Entities.CandidateModule
{
    public class CandidateProfile : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Step 2 - Professional Profile
        public string? PhoneNumber { get; set; }
        public string? CurrentJobTitle { get; set; }
        public string? CurrentCompany { get; set; }
        public int? TotalYearsOfExperience { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? PortfolioUrl { get; set; }

        // Step 3 - Resume
        public string? ResumeUrl { get; set; }
        public string? ResumeFileName { get; set; }

        // Step 4 - Preferences
        public string? PreferredLocation { get; set; }
        public bool RemoteOnly { get; set; } = false;
        public decimal? MinSalaryExpectation { get; set; }
        public decimal? MaxSalaryExpectation { get; set; }
        public string? Currency { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public WorkAuthorization? WorkAuthorization { get; set; }

        public ICollection<CandidateProfileSkill> Skills { get; set; } = new List<CandidateProfileSkill>();
    }
}