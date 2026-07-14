using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Interfaces;

namespace TalentFlow.Application.Features.User.Query.GetUser
{
    public class GetUsersQueryHandler : IRequestHandler< GetUsersQuery, List<GetUsersDTOs>>
    {

        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public GetUsersQueryHandler(
            UserManager<Domain.Entities.IdentityModule.User> userManager,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }
        public async Task<List<GetUsersDTOs>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.Where(x => x.TenantId == _currentUserService.TenantId).ToListAsync(cancellationToken);
            if (users == null)
            {
                throw new Exception("Users not  found");
            }
            var result = new List<GetUsersDTOs>();
           foreach (var user in users)
            {

            var roles = await _userManager.GetRolesAsync(user);
                result.Add( new GetUsersDTOs
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                Roles = roles.ToList()
            });
        }
            return result;
    }
}
            }
