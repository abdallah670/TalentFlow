using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Roles.Command.CreateRole
{
    public class CreateRoleCommandHandler: IRequestHandler<CreateRoleCommand,BaseCommandResponse<bool>>
    {
        private readonly ILogger<CreateRoleCommandHandler> logger;

        private readonly RoleManager<Role> roleManager;

        public CreateRoleCommandHandler(RoleManager<Role> roleManager, ILogger<CreateRoleCommandHandler> logger)
        {
            this.logger = logger;
            this.roleManager = roleManager;
        }

        public async Task<BaseCommandResponse<bool>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {

            logger.LogInformation("Handling {Handler}", nameof(CreateRoleCommandHandler));
            if (await roleManager.RoleExistsAsync(request.Name))
            {
                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = "Role already exists."
                };
            }
            var res = await roleManager.CreateAsync(new Role
            {
                Name = request.Name,
            });
            if (!res.Succeeded)
            {
                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = string.Join(", ", res.Errors.Select(x => x.Description))
                };
            }

            return new BaseCommandResponse<bool>
            {
                Success = true,
                Message = "Role created successfully."
            };
        }
    }
}
