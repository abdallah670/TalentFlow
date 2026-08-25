using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentFlow.Application.Features.Departments.Commands.CreateDepartment;
using TalentFlow.Application.Features.Departments.Commands.DeleteDepartment;
using TalentFlow.Application.Features.Departments.Commands.UpdateDepartment;
using TalentFlow.Application.Features.Departments.Queries.GetDepartmentById;
using TalentFlow.Application.Features.Departments.Queries.GetDepartments;

namespace TalentFlow.Api.Controller
{
    [Authorize(Roles = "TenantAdmin")]

    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public DepartmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }



        ////GET    /api/v1/departments

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var res = await mediator.Send(new GetDepartmentsQuery());


            return Ok(res);
        }
        ////GET    /api/v1/departments/{id}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(Guid id)
        {
            var res = await mediator.Send(new GetDepartmentQuery { Id = id });
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }
        ////POST   /api/v1/departments


        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentCommand command)
        {
            var res = await mediator.Send(command);
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }
        ////PUT    /api/v1/departments/{id}

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(Guid id,[FromBody] UpdateDepartmentCommand command)
        {
            command.id = id;
            var res = await mediator.Send(command);
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }
        ////DELETE /api/v1/departments/{id}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            var res = await mediator.Send(new DeleteDepartmentCommand { Id=id});
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

    }
}
