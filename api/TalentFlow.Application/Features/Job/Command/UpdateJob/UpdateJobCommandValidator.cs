using FluentValidation;

namespace TalentFlow.Application.Features.Job.Command.UpdateJob
{
    public class UpdateJobCommandValidator : AbstractValidator<UpdateJobCommand>
    {
        public UpdateJobCommandValidator()
        {
            RuleFor(x => x.DepartmentId)
                .NotEmpty()
                .WithMessage("Department is required.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Job title is required and must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(5000)
                .WithMessage("Job description is required and must not exceed 5000 characters.");

            RuleFor(x => x.SalaryMin)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Minimum salary cannot be negative.");

            RuleFor(x => x.SalaryMax)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Maximum salary cannot be negative.");

            RuleFor(x => x)
                .Must(x => x.SalaryMin <= x.SalaryMax)
                .WithMessage("Minimum salary cannot be greater than maximum salary.");

            RuleFor(x => x.OpenDate)
                .NotEmpty()
                .WithMessage("Open date is required.");

            RuleFor(x => x)
                .Must(x => !x.CloseDate.HasValue || x.CloseDate.Value >= x.OpenDate)
                .WithMessage("Close date cannot be earlier than open date.");
        }
    }
}