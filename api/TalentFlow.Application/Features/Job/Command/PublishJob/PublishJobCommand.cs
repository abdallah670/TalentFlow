using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Job.Command.PublishJob
{
    public class PublishJobCommand : IRequest<BaseCommandResponse>
    {
        [JsonIgnore]
        public Guid JobId { get; set; }
       
    }
}
