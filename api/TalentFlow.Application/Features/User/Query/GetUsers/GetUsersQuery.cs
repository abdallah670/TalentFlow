using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.User.Query.GetUser
{
    public class GetUsersQuery : IRequest<List<GetUsersDTOs>>
    {

    }
}
