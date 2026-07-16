    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using TalentFlow.Application.Features.User.Query.GetUser;
    using TalentFlow.Application.Interfaces;
    using TalentFlow.Application.Responses;

    namespace TalentFlow.Application.Features.User.Command.DisableUser
    {
        public class DisableUserHandler : IRequestHandler<DisAbleUserCommand, BaseCommandResponse>
        {
            private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;
            private readonly ICurrentUserService _currentUserService;

            public DisableUserHandler(UserManager<Domain.Entities.IdentityModule.User> userManager, ICurrentUserService currentUserService)
            {
                _userManager = userManager;
                _currentUserService = currentUserService;
            }

            public async Task<BaseCommandResponse> Handle(DisAbleUserCommand request, CancellationToken cancellationToken)
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
               
            if (!user.IsActive)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "User is already disabled."
                };
            }
            user.IsActive = false;
                await _userManager.UpdateAsync(user);
            var dto = new GetUserDTOs
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles.ToList()
            };
            return new BaseCommandResponse
                {
                    Success = true,
                    Message = "User disabled successfully.",
                    Id = user.Id,
                    Data = dto
                };

            }
        }
    }
