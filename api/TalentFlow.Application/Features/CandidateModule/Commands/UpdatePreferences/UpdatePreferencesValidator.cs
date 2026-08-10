using FluentValidation;

namespace TalentFlow.Application.Features.CandidateModule.Commands.UpdatePreferences
{
    public class UpdatePreferencesValidator : AbstractValidator<UpdatePreferencesCommand>
    {
        public UpdatePreferencesValidator()
        {
            RuleFor(x => x.MaxSalaryExpectation)
                .GreaterThanOrEqualTo(x => x.MinSalaryExpectation)
                .When(x => x.MinSalaryExpectation.HasValue && x.MaxSalaryExpectation.HasValue)
                .WithMessage("Maximum salary must be greater than or equal to minimum salary.");

            RuleFor(x => x.Currency)
                .MaximumLength(10)
                .When(x => !string.IsNullOrEmpty(x.Currency));

            RuleFor(x => x.AvailableFrom)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .When(x => x.AvailableFrom.HasValue)
                .WithMessage("Available from date cannot be in the past.");
        }
    }
}