using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using TalentFlow.Application.Features.Job.DTOs;
using TalentFlow.Application.Models;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Enums;

namespace TalentFlow.Application.Features.Job.Queries.GetJobsCompany
{
    public class GetJobsCompanyQuery : IRequest<BaseCommandResponse<PaginatedResult<GetJobDto>>>
    {
        
        public EmploymentType? EmploymentType { get; set; }

        public ExperienceLevel? ExperienceLevel { get; set; }

        public JobStatus? Status { get; set; }

        public Guid? DepartmentId { get; set; }
        public int PageNumber { get; set; }=1;
        public int PageSize { get; set;} = 10;
        public string? Search { get; set; }
    }
}
