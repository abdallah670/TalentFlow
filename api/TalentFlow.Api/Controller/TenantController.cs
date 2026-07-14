using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentFlow.Application.Features.Tenant.Command.RegisterTenant;

namespace TalentFlow.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly IMediator mediator;

        public TenantController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("Regisret_Tenant")]
        public async Task<IActionResult> Register([FromBody] TenantRegisterCommand command)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result =await mediator.Send(command);
            return Ok(result);
        }
    }
}
