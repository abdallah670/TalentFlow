using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.User.Query.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, BaseCommandResponse>
    {
        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public GetUserQueryHandler(UserManager<Domain.Entities.IdentityModule.User> userManager, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<BaseCommandResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x=>x.Id == request.Id,cancellationToken );

            if (user == null)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "User Not Found"
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
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
                Data = dto,
                Message = "User retrieved successfully.",

                Id = request.Id,
                
            };

        }
    }
}
