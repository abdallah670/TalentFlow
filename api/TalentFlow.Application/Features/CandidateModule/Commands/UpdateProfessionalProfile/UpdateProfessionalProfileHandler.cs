// TalentFlow.Application/Features/CandidateModule/Commands/UpdateProfessionalProfile/UpdateProfessionalProfileHandler.cs
using MediatR;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.CandidateModule;

namespace TalentFlow.Application.Features.CandidateModule.Commands.UpdateProfessionalProfile
{
    public class UpdateProfessionalProfileHandler : IRequestHandler<UpdateProfessionalProfileCommand, UpdateProfessionalProfileResponse>
    {
        private readonly IcandidateProfileRepo candidateProfileRepo;

        public UpdateProfessionalProfileHandler(IcandidateProfileRepo candidateProfileRepo)
        {
            this.candidateProfileRepo = candidateProfileRepo;
        }

        public async Task<UpdateProfessionalProfileResponse> Handle(UpdateProfessionalProfileCommand request, CancellationToken cancellationToken)
        {
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

            return new UpdateProfessionalProfileResponse
            {
                Success = true,
                Message = "Professional profile updated successfully."
            };
        }
    }
}