using MediatR;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Interfaces;
using TalentFlow.Domain.Entities.CandidateModule;

namespace TalentFlow.Application.Features.CandidateModule.Commands.UploadResume
{
    public class UploadResumeHandler : IRequestHandler<UploadResumeCommand, UploadResumeResponse>
    {
        private readonly IcandidateProfileRepo candidateProfileRepo;
        private readonly IFileStorageService fileStorageService;

        public UploadResumeHandler(IcandidateProfileRepo candidateProfileRepo, IFileStorageService fileStorageService)
        {
            this.candidateProfileRepo = candidateProfileRepo;
            this.fileStorageService = fileStorageService;
        }

        public async Task<UploadResumeResponse> Handle(UploadResumeCommand request, CancellationToken cancellationToken)
        {
            var existingProfiles = await candidateProfileRepo.FindAsync(x => x.UserId == request.UserId);
            var profile = existingProfiles.FirstOrDefault();

            var uploadResult = await fileStorageService.UploadAsync(request.File, "resumes", cancellationToken);

            if (profile is null)
            {
                profile = new CandidateProfile
                {
                    UserId = request.UserId,
                    ResumeUrl = uploadResult.Url,
                    ResumeFileName = request.File.FileName
                };

                await candidateProfileRepo.AddAsync(profile);
            }
            else
            {
                profile.ResumeUrl = uploadResult.Url;
                profile.ResumeFileName = request.File.FileName;

                await candidateProfileRepo.UpdateAsync(profile);
            }

            await candidateProfileRepo.SaveAsync(cancellationToken);

            return new UploadResumeResponse
            {
                Success = true,
                Message = "Resume uploaded successfully.",
                ResumeUrl = profile.ResumeUrl
            };
        }
    }
}