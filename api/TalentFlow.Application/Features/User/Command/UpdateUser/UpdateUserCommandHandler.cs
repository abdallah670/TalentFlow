using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Features.User.Query.GetUser;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.User.Command.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, BaseCommandResponse>
    {
        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public UpdateUserCommandHandler(UserManager<Domain.Entities.IdentityModule.User> userManager, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }
        public async Task<BaseCommandResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (user == null)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "User Not Found"
                };
            }
            var roles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, request.Role);
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.UserName = request.UserName;
            user.Email = request.Email;


          var result=  await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "Failed to update user"
                };
            }
            var newRoles = await _userManager.GetRolesAsync(user);

            var dtos = new GetUserDTOs
            {
                Id = request.Id,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Roles = newRoles,

            };
            return new BaseCommandResponse
            {
                Success = true,
                Data = dtos,
                Id = request.Id,
            };
        }
    }
}
