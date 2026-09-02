using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.User.Command.UpdateUserRole
{
    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, BaseCommandResponse<bool>>
    {
        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;
        private readonly RoleManager<Domain.Entities.IdentityModule.Role> _roleManager;
        private readonly IRefreshTokenService _refreshTokenService;

        public UpdateUserRoleCommandHandler(
            UserManager<Domain.Entities.IdentityModule.User> userManager,
            RoleManager<Domain.Entities.IdentityModule.Role> roleManager,
            IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<BaseCommandResponse<bool>> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (user is null)
            {
                return new BaseCommandResponse<bool> { Success = false, Message = "User Not Found" };
            }

            var roleExists = await _roleManager.RoleExistsAsync(request.Role);
            if (!roleExists)
            {
                return new BaseCommandResponse<bool> { Success = false, Message = $"Role '{request.Role}' does not exist." };
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Contains(request.Role))
            {
                return new BaseCommandResponse<bool> { Success = false, Message = "User already has this role." };
            }

            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
            }

            var addResult = await _userManager.AddToRoleAsync(user, request.Role);

            if (!addResult.Succeeded)
            {
                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = string.Join(", ", addResult.Errors.Select(x => x.Description))
                };
            }

            // ‰·€Ì «· Êﬂ‰«  «·ﬁœÌ„… ⁄‘«‰ «·‹ role «·ÃœÌœ Ì ›⁄· ›Ê—« „‘ »⁄œ „« «· Êﬂ‰ «·ﬁœÌ„ Ì‰ ÂÌ
            await _refreshTokenService.RevokeAllRefreshTokensForUserAsync(user.Id);

            return new BaseCommandResponse<bool>
            {
                Success = true,
                Message = "User role updated successfully.",
                Id = user.Id
            };
        }
    }
}