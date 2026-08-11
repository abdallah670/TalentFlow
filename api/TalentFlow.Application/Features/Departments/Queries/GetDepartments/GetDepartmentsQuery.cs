using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Departments.Queries.GetDepartments
{
    public class GetDepartmentsQuery :IRequest<BaseCommandResponse>
    {
    }
}
