using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Departments.DTOs
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
