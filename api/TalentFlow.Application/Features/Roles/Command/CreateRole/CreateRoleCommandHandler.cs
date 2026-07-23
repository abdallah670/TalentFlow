using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.IdentityModule;

namespace TalentFlow.Application.Features.Roles.Command.CreateRole
{
    public class CreateRoleCommandHandler: IRequestHandler<CreateRoleCommand,BaseCommandResponse>
    {

        private readonly RoleManager<Role> roleManager;

        public CreateRoleCommandHandler(RoleManager<Role> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task<BaseCommandResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {

            if (await roleManager.RoleExistsAsync(request.Name))
            {
                return new BaseCommandResponse
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
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = string.Join(", ", res.Errors.Select(x => x.Description))
                };
            }

            return new BaseCommandResponse
            {
                Success = true,
                Message = "Role created successfully."
            };
        }
    }
}
