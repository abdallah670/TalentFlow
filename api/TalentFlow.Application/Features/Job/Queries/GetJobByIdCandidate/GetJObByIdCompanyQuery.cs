using MediatR;
using System;
using TalentFlow.Application.Features.Job.DTOs;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Job.Queries.GetJobById
{
    public class GetJObByIdCompanyQuery : IRequest<BaseCommandResponse<GetJobDto>>
    {
        public Guid JobId { get; set; }
    }
}
