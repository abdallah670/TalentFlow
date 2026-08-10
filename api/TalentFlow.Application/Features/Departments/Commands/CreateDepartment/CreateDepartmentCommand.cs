using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentCommand :IRequest<BaseCommandResponse>
    {
        public string Name { get; set; } = default!;

    }
}
