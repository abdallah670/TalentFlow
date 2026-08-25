using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TalentFlow.Application.Features.Job.Queries.GetJobsForCandidate
{
    public class GetJobsForCandidateValidator :AbstractValidator<GetJobsForCandidateQuery>
    {
        public GetJobsForCandidateValidator() 
        {
            RuleFor(x => x.PageNumber)
    .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);
        }
    }
}
