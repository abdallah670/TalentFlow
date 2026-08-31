using MediatR;
using Microsoft.AspNetCore.Identity;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.User.Command.CreateUser
{
    public class CreateUserCommandHandler
        : IRequestHandler<CreateUserCommand, BaseCommandResponse>
    {
        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public CreateUserCommandHandler(
            UserManager<Domain.Entities.IdentityModule.User> userManager,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<BaseCommandResponse> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            // =========================
            // Validate Tenant
            // =========================

            if (_currentUserService.TenantId == Guid.Empty)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "Tenant not found."
                };
            }

            // =========================
            // Check Email
            // =========================

            var existingUser =
                await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "Email already exists."
                };
            }

            // =========================
            // Check Username
            // =========================

            var existingUserName =
                await _userManager.FindByNameAsync(request.UserName);

            if (existingUserName != null)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "Username already exists."
                };
            }

            // =========================
            // Create User
            // =========================

            var user = new Domain.Entities.IdentityModule.User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Email = request.Email,
                TenantId = _currentUserService.TenantId,
                IsActive = true,

                // لأنه اتعمل من داخل الـ Admin
                EmailConfirmed = true
            };

            var createResult =
                await _userManager.CreateAsync(
                    user,
                    request.Password);

            if (!createResult.Succeeded)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = string.Join(
                        ", ",
                        createResult.Errors.Select(x => x.Description))
                };
            }

            // =========================
            // Assign Role
            // =========================

            var roleResult =
                await _userManager.AddToRoleAsync(
                    user,
                    request.Role);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                return new BaseCommandResponse
                {
                    Success = false,
                    Message = string.Join(
                        ", ",
                        roleResult.Errors.Select(x => x.Description))
                };
            }

            return new BaseCommandResponse
            {
                Success = true,
                Id = user.Id,
                Message =
                    $"User created as {request.Role} successfully."
            };
        }
    }
}