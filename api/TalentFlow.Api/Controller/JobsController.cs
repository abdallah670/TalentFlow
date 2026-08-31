using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentFlow.Application.Features.Job.Command.CloseJob;
using TalentFlow.Application.Features.Job.Command.CreateJob;
using TalentFlow.Application.Features.Job.Command.PublishJob;
using TalentFlow.Application.Features.Job.Command.UpdateJob;
using TalentFlow.Application.Features.Job.Queries.GetJobById;
using TalentFlow.Application.Features.Job.Queries.GetJobsCompany;
using TalentFlow.Application.Features.Job.Queries.GetJobsForCandidate;
using TalentFlow.Domain.Entities.IdentityModule;
using TalentFlow.Domain.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TalentFlow.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobsController : ControllerBase
    {
        private readonly IMediator mediator;

        public JobsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        //POST /api/Jobs

        [HttpPost]
        [Authorize(Roles = nameof(Roles.Recruiter))]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await mediator.Send(command);
            return Ok(res);
        }
       // PUT /api/Jobs/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Roles.Recruiter))]
        public async Task<IActionResult> UpdateJob(Guid id, [FromBody] UpdateJobCommand command)
        {
            command.JobId = id;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await mediator.Send(command);
            return Ok(res);
        }
        // PATCH /api/Jobs/{id}/publish
        [HttpPatch("{id}/publish")]
        [Authorize(Roles = nameof(Roles.Recruiter))]
        public async Task<IActionResult> PublishJob(Guid id, [FromBody] PublishJobCommand command)
        {
            command.JobId = id;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await mediator.Send(command);
            return Ok(res);
        }
        // PATCH /api/Jobs/{id}/close
        [HttpPatch("{id}/close")]
        [Authorize(Roles = nameof(Roles.Recruiter))]
        public async Task<IActionResult> CloseJob(Guid id, [FromBody] CloseJobCommand command)
        {
            command.JobId = id;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await mediator.Send(command);
            return Ok(res);
        }
    
      //  GET /api/Jobs/company



     [HttpGet("company")]
        [Authorize(Roles = nameof(Roles.Recruiter))]
        public async Task<IActionResult> GetJobsForCompany(string? Search, int Page = 1, int PageSize = 10, JobStatus? Status=null, EmploymentType? EmploymentType=null, Guid? DepartmentId=null, ExperienceLevel? ExperienceLevel=null)
        {
            var res = await mediator.Send(new GetJobsCompanyQuery
            {
                Search = Search,
                PageNumber = Page,
                PageSize = PageSize,
                Status = Status,
                ExperienceLevel = ExperienceLevel,
                EmploymentType = EmploymentType,
                DepartmentId = DepartmentId
            });
            return Ok(res);
        }
        // GET /api/Jobs/candidate


        [HttpGet("candidate")]
        [Authorize(Roles = nameof(Roles.Candidate))]
        public async Task<IActionResult> GetJobsForCandidate(string? Search, int Page = 1, int PageSize = 10, EmploymentType? EmploymentType = null, Guid? DepartmentId = null, ExperienceLevel? ExperienceLevel = null)
        {
            var res = await mediator.Send(new GetJobsForCandidateQuery
            {
                Search = Search,
                PageNumber = Page,
                PageSize = PageSize,
                ExperienceLevel = ExperienceLevel,
                EmploymentType = EmploymentType,
                DepartmentId = DepartmentId
            });
            return Ok(res);
        }
        // GET /api/Jobs/company/{id
        [HttpGet("company/{id}")]
        [Authorize(Roles = nameof(Roles.Recruiter))]
        public async Task<IActionResult> GetJobByIdForCompany(Guid id)
        {
            var res = await mediator.Send(new GetJObByIdCompanyQuery { JobId = id });
            return Ok(res);
        }
        // GET /api/Jobs/candidate/{id


        [HttpGet("candidate/{id}")]
        [Authorize(Roles = nameof(Roles.Candidate))]
        public async Task<IActionResult> GetJobByIdForCandidate(Guid id)
        {
            var res = await mediator.Send(new GetJObByIdCandiateQuery { JobId = id });
            return Ok(res);
        }
    }
}               
                     

