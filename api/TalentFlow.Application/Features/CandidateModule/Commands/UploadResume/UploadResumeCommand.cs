using MediatR;
using Microsoft.AspNetCore.Http;

namespace TalentFlow.Application.Features.CandidateModule.Commands.UploadResume
{
    public class UploadResumeCommand : IRequest<UploadResumeResponse>
    {
        public Guid UserId { get; set; }
        public IFormFile File { get; set; } = default!;
    }

    public class UploadResumeResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = default!;
        public string? ResumeUrl { get; set; }
    }
}