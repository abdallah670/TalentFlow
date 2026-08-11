using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Departments.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommand :IRequest<BaseCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
