using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommand :IRequest<BaseCommandResponse<bool>>
    {
        public Guid id { get; set; }
        public string name { get; set; } = default!;
    }
}
