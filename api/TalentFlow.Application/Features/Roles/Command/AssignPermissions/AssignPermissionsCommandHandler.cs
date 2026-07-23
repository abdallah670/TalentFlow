using MediatR;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Roles.Command.AssignPermissions
{
    public class AssignPermissionsCommandHandler
        : IRequestHandler<AssignPermissionsCommand, BaseCommandResponse>
    {
        private readonly IPermissionRepository permissionRepository;
        private readonly IRolePermissionRepository rolePermissionRepository ;

       

        public AssignPermissionsCommandHandler(IPermissionRepository permissionRepository, IRolePermissionRepository rolePermissionRepository)
        {
            this.permissionRepository = permissionRepository;
            this.rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<BaseCommandResponse> Handle(
            AssignPermissionsCommand request,
            CancellationToken cancellationToken)
        {
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

            return new BaseCommandResponse
            {
                Success = true,
                Message = "Permissions assigned successfully."
            };
        }
    }
}