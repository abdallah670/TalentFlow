using FluentValidation;

namespace TalentFlow.Application.Features.CandidateModule.Commands.UpdateProfessionalProfile
{
    public class UpdateProfessionalProfileValidator : AbstractValidator<UpdateProfessionalProfileCommand>
    {
        public UpdateProfessionalProfileValidator()
        {
            RuleFor(x => x.TotalYearsOfExperience)
                .InclusiveBetween(0, 50)
                .When(x => x.TotalYearsOfExperience.HasValue)
                .WithMessage("Years of experience must be between 0 and 50.");

            RuleFor(x => x.LinkedInUrl)
                .Must(BeAValidUrl)
                .When(x => !string.IsNullOrEmpty(x.LinkedInUrl))
                .WithMessage("LinkedIn URL must be a valid URL.");

            RuleFor(x => x.PortfolioUrl)
                .Must(BeAValidUrl)
                .When(x => !string.IsNullOrEmpty(x.PortfolioUrl))
                .WithMessage("Portfolio URL must be a valid URL.");
        }

        private bool BeAValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}