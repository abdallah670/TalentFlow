using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Roles.Query.GetRoles
{
    
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, BaseCommandResponse>
    {
        private readonly RoleManager<Domain.Entities.IdentityModule.Role> roleManager;

        public GetRolesQueryHandler(RoleManager<Domain.Entities.IdentityModule.r> roleManager)
        {
            this.roleManager = roleManager;
        }

        
public async Task<BaseCommandResponse> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var res = await roleManager.Roles
        .Select(x => new RoleDto
        {
            Id = x.Id.ToString(),
            Name = x.Name!
        })
        .ToListAsync(cancellationToken);

            return new BaseCommandResponse
            {
                Success = true,
                Data = res,

            };
        }
          
}
}
