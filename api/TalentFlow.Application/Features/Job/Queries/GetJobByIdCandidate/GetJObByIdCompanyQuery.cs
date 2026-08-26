using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Features.Job.DTOs;

namespace TalentFlow.Application.Features.Job.Queries.GetJobById
{
    public class GetJObByIdCompanyQuery : IRequest<GetJobDto>
    {
        public Guid JobId { get; set; }
    }
}
