using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Roles.Query.GetRoles
{
    
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, BaseCommandResponse<List<RoleDto>>>
    {
        private readonly ILogger<GetRolesQueryHandler> logger;
        private readonly RoleManager<Domain.Entities.IdentityModule.Role> roleManager;

        public GetRolesQueryHandler(RoleManager<Domain.Entities.IdentityModule.Role> roleManager, ILogger<GetRolesQueryHandler> logger)
        {
            this.logger = logger;
            this.roleManager = roleManager;
        }

        
public async Task<BaseCommandResponse<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(GetRolesQueryHandler));
            var res = await roleManager.Roles
        .Select(x => new RoleDto
        {
            Id = x.Id.ToString(),
            Name = x.Name!
        })
        .ToListAsync(cancellationToken);

            return new BaseCommandResponse<List<RoleDto>>
            {
                Success = true,
                Data = res,

            };
        }
          
}
}
