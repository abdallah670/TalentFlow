// TalentFlow.Application/Features/CandidateModule/Commands/UploadResume/UploadResumeValidator.cs
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace TalentFlow.Application.Features.CandidateModule.Commands.UploadResume
{
    public class UploadResumeValidator : AbstractValidator<UploadResumeCommand>
    {
        private static readonly string[] AllowedExtensions = { ".pdf", ".docx" };
        private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10MB

        public UploadResumeValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("Resume file is required.");

            RuleFor(x => x.File)
                .Must(f => f.Length <= MaxFileSizeBytes)
                .When(x => x.File is not null)
                .WithMessage("Resume file must not exceed 10MB.");

            RuleFor(x => x.File)
                .Must(HaveValidExtension)
                .When(x => x.File is not null)
                .WithMessage("Resume must be a PDF or DOCX file.");
        }

        private bool HaveValidExtension(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            return AllowedExtensions.Contains(extension);
        }
    }
}