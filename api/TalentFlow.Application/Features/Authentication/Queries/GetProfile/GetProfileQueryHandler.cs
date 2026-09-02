using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Authentication.Queries.GetProfile
{
    public class GetProfileQueryHandler
        : IRequestHandler<GetProfileQuery, BaseCommandResponse<UserProfileDto>>
    {
        private readonly ILogger<GetProfileQueryHandler> logger;
        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public GetProfileQueryHandler(
            UserManager<Domain.Entities.IdentityModule.User> userManager,
            ICurrentUserService currentUserService, ILogger<GetProfileQueryHandler> logger)
        {
            this.logger = logger;
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<BaseCommandResponse<UserProfileDto>> Handle(
            GetProfileQuery request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(GetProfileQueryHandler));
            var user = await _userManager.FindByIdAsync(
                _currentUserService.UserId.ToString());

            if (user is null)
            {
                return new BaseCommandResponse<UserProfileDto>
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new BaseCommandResponse<UserProfileDto>
            {
                Success = true,
                Data = new UserProfileDto
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = roles.ToList()
                }
            };
        }
    }
}