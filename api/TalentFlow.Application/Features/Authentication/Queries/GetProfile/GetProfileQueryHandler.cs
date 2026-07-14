using MediatR;
using Microsoft.AspNetCore.Identity;
using TalentFlow.Application.Interfaces;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Authentication.Queries.GetProfile
{
    public class GetProfileQueryHandler
        : IRequestHandler<GetProfileQuery, UserProfileDto>
    {
        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public GetProfileQueryHandler(
            UserManager<Domain.Entities.IdentityModule.User> userManager,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<UserProfileDto> Handle(
            GetProfileQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(
                _currentUserService.UserId.ToString());

            if (user is null)
                throw new Exception("User not found");

            var roles = await _userManager.GetRolesAsync(user);

            return new UserProfileDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToList()
            };
        }
    }
}