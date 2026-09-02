using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Models.Identity;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.IdentityModule;
using TalentFlow.Domain.Entities.TenantModule;
using TalentFlow.Domain.Enums;

namespace TalentFlow.Application.Features.User.Command.CreateUser
{
    public class CreateUserCommandHandler :IRequestHandler<CreateUserCommand,BaseCommandResponse<bool>>
    {
        private readonly ILogger<CreateUserCommandHandler> logger;
        private readonly UserManager<Domain.Entities.IdentityModule.User> userManager;
        private readonly IJWTService jWTService;
        private readonly ICurrentUserService currentUserService;

        public CreateUserCommandHandler(UserManager<Domain.Entities.IdentityModule.User> userManager, IJWTService jWTService, ICurrentUserService currentUserService, ILogger<CreateUserCommandHandler> logger)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.jWTService = jWTService;
            this.currentUserService = currentUserService;
        }

        public async Task<BaseCommandResponse<bool>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(CreateUserCommandHandler));
            var existinguser = await userManager.FindByEmailAsync(request.Email);
            if (existinguser is not null)
            {
                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    

                    Message = "Email Already Exist"
                };
            }
            var existingUserName = await userManager.FindByNameAsync(request.UserName);

            if (existingUserName != null)
            {
                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = "Username already exists."
                };
            }
            var user = new Domain.Entities.IdentityModule.User
            {
                FirstName = request.FirstName,
                UserName = request.UserName,
                Email = request.Email,
                TenantId = currentUserService.TenantId,
                
                LastName = request.LastName,
                IsActive = true
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(x => x.Description))
                };
            }


          //  await userManager.AddToRoleAsync(user, request.Role);
            var roleResult = await userManager.AddToRoleAsync(user, request.Role);

            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(user);

                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = string.Join(", ", roleResult.Errors.Select(x => x.Description))
                };
            }
            var roles = await userManager.GetRolesAsync(user);




            return new BaseCommandResponse<bool>
            {
                Success = true,
                Id = user.Id,
                Message =$"User created as a{request.Role} successfully ."
            };
        }
    }
}
