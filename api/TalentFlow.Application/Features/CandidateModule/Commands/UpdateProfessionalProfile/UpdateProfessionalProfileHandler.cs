// TalentFlow.Application/Features/CandidateModule/Commands/UpdateProfessionalProfile/UpdateProfessionalProfileHandler.cs
using MediatR;
using Microsoft.Extensions.Logging;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.CandidateModule;

namespace TalentFlow.Application.Features.CandidateModule.Commands.UpdateProfessionalProfile
{
    public class UpdateProfessionalProfileHandler : IRequestHandler<UpdateProfessionalProfileCommand, BaseCommandResponse<bool>>
    {
        private readonly ILogger<UpdateProfessionalProfileHandler> logger;
        private readonly IcandidateProfileRepo candidateProfileRepo;

        public UpdateProfessionalProfileHandler(IcandidateProfileRepo candidateProfileRepo, ILogger<UpdateProfessionalProfileHandler> logger)
        {
            this.logger = logger;
            this.candidateProfileRepo = candidateProfileRepo;
        }

        public async Task<BaseCommandResponse<bool>> Handle(UpdateProfessionalProfileCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(UpdateProfessionalProfileHandler));
            var existingProfiles = await candidateProfileRepo.FindAsync(x => x.UserId == request.UserId);
            var profile = existingProfiles.FirstOrDefault();

            if (profile is null)
            {
                profile = new CandidateProfile { UserId = request.UserId };
                profile.PhoneNumber = request.PhoneNumber;
                profile.CurrentJobTitle = request.CurrentJobTitle;
                profile.CurrentCompany = request.CurrentCompany;
                profile.TotalYearsOfExperience = request.TotalYearsOfExperience;
                profile.LinkedInUrl = request.LinkedInUrl;
                profile.PortfolioUrl = request.PortfolioUrl;

                await candidateProfileRepo.AddAsync(profile);
            }
            else
            {
                profile.PhoneNumber = request.PhoneNumber;
                profile.CurrentJobTitle = request.CurrentJobTitle;
                profile.CurrentCompany = request.CurrentCompany;
                profile.TotalYearsOfExperience = request.TotalYearsOfExperience;
                profile.LinkedInUrl = request.LinkedInUrl;
                profile.PortfolioUrl = request.PortfolioUrl;

                await candidateProfileRepo.UpdateAsync(profile);
            }

            await candidateProfileRepo.SaveAsync(cancellationToken);

            return new BaseCommandResponse<bool>
            {
                Success = true,
                Message = "Professional profile updated successfully.",
                Data = true
            };
        }
    }
}