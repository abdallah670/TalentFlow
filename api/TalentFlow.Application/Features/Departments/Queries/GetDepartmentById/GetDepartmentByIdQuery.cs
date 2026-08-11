using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentQuery :IRequest<BaseCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
