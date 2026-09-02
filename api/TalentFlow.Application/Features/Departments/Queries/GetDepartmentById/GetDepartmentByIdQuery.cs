using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Features.Departments.DTOs;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentQuery :IRequest<BaseCommandResponse<DepartmentDto>>
    {
        public Guid Id { get; set; }
    }
}
