using MediatR;
using Microsoft.Extensions.Logging;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Roles.Command.AssignPermissions
{
    public class AssignPermissionsCommandHandler
        : IRequestHandler<AssignPermissionsCommand, BaseCommandResponse<bool>>
    {
        private readonly ILogger<AssignPermissionsCommandHandler> logger;
        private readonly IPermissionRepository permissionRepository;
        private readonly IRolePermissionRepository rolePermissionRepository ;

       

        public AssignPermissionsCommandHandler(IPermissionRepository permissionRepository, IRolePermissionRepository rolePermissionRepository, ILogger<AssignPermissionsCommandHandler> logger)
        {
            this.logger = logger;
            this.permissionRepository = permissionRepository;
            this.rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<BaseCommandResponse<bool>> Handle(
            AssignPermissionsCommand request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(AssignPermissionsCommandHandler));
            var permissions = await permissionRepository.FindAsync(
       x => request.PermissionIds.Contains(x.Id));

            var oldPermissions = await rolePermissionRepository.FindAsync(
                x => x.RoleId == request.RoleId);

            foreach (var item in oldPermissions)
            {
                await rolePermissionRepository.DeleteAsync(item);
            }

            foreach (var permission in permissions)
            {
                await rolePermissionRepository.AddAsync(new RolePermission
                {
                    RoleId = request.RoleId,
                    PermissionId = permission.Id
                });
            }

            return new BaseCommandResponse<bool>
            {
                Success = true,
                Message = "Permissions assigned successfully."
            };
        }
    }
}