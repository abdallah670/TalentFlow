using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Enums;

namespace TalentFlow.Application.Features.Job.Command.UpdateJob
{
    public class UpdateJobCommand :IRequest <BaseCommandResponse<bool>>
    {
        [JsonIgnore]
        public Guid JobId { get; set; }
        [JsonIgnore]
        public Guid TenantId { get; set; }

        public Guid DepartmentId { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public EmploymentType EmploymentType { get; set; } = default!;
        public ExperienceLevel ExperienceLevel { get; set; } = default!;
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
    
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }

    }
}
