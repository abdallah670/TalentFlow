// TalentFlow.Application/Features/CandidateModule/Commands/UpdateProfessionalProfile/UpdateProfessionalProfileCommand.cs
using MediatR;

namespace TalentFlow.Application.Features.CandidateModule.Commands.UpdateProfessionalProfile
{
    public class UpdateProfessionalProfileCommand : IRequest<UpdateProfessionalProfileResponse>
    {
        public Guid UserId { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? CurrentJobTitle { get; set; }
        public string? CurrentCompany { get; set; }
        public int? TotalYearsOfExperience { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? PortfolioUrl { get; set; }
    }

    public class UpdateProfessionalProfileResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = default!;
    }
}