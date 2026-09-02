using MediatR;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.User.Query.GetUser
{
    public class GetUsersQuery : IRequest<BaseCommandResponse<CursorPagination.CursorPagedResult<GetUsersDTOs>>>
    {
        /// <summary>
        /// Opaque base64 cursor returned by the previous page (null = first page).
        /// </summary>
        public string? Cursor { get; set; }

        /// <summary>Number of items to return (clamped between 1 and 100).</summary>
        public int PageSize { get; set; } = 20;
    }
}
