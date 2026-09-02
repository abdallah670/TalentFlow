using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.User.Query.GetUser
{
    public class GetUsersQueryHandler
        : IRequestHandler<GetUsersQuery, BaseCommandResponse<CursorPagination.CursorPagedResult<GetUsersDTOs>>>
    {
        private readonly ILogger<GetUsersQueryHandler> logger;
        private readonly UserManager<Domain.Entities.IdentityModule.User> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public GetUsersQueryHandler(
            UserManager<Domain.Entities.IdentityModule.User> userManager,
            ICurrentUserService currentUserService,
            ILogger<GetUsersQueryHandler> logger)
        {
            this.logger = logger;
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<BaseCommandResponse<CursorPagination.CursorPagedResult<GetUsersDTOs>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(GetUsersQueryHandler));
            try
            {
                var pageSize = request.PageSize <= 0 ? 20 : Math.Min(request.PageSize, 100);

                var usersQuery = _userManager.Users
                    .Where(x => x.TenantId == _currentUserService.TenantId);

                // Keyset (cursor) filtering: continue strictly after the decoded cursor.
                var cursorData = CursorPagination.DecodeCursor(request.Cursor);
                if (cursorData is not null)
                {
                    usersQuery = usersQuery.Where(u =>
                        u.CreatedAt > cursorData.CreatedAt ||
                        (u.CreatedAt == cursorData.CreatedAt && string.Compare(u.Id.ToString(), cursorData.Id.ToString()) > 0));
                }

                // Fetch one extra row to know if another page exists.
                var users = await usersQuery
                    .OrderBy(u => u.CreatedAt)
                    .ThenBy(u => u.Id)
                    .Take(pageSize + 1)
                    .ToListAsync(cancellationToken);

                var hasMore = users.Count > pageSize;
                if (hasMore)
                    users = users.Take(pageSize).ToList();

                var result = new List<GetUsersDTOs>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    result.Add(new GetUsersDTOs
                    {
                        Id = user.Id,
                        UserName = user.UserName!,
                        Email = user.Email!,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        IsActive = user.IsActive,
                        CreatedAt = user.CreatedAt,
                        Roles = roles.ToList()
                    });
                }

                var last = users.LastOrDefault();
                var pagedResult = new CursorPagination.CursorPagedResult<GetUsersDTOs>
                {
                    Items = result,
                    PageSize = pageSize,
                    HasMore = hasMore,
                    NextCursor = hasMore && last != null
                        ? CursorPagination.CreateCursor(last.CreatedAt, last.Id)
                        : null
                };

                return new BaseCommandResponse<CursorPagination.CursorPagedResult<GetUsersDTOs>>
                {
                    Success = true,
                    Data = pagedResult
                };
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "Failed to get users in {Handler}", nameof(GetUsersQueryHandler));
                return new BaseCommandResponse<CursorPagination.CursorPagedResult<GetUsersDTOs>>
                {
                    Success = false,
                    Message = "Failed to retrieve users.",
                    Errors = { ex.Message }
                };
            }
        }
    }
}
