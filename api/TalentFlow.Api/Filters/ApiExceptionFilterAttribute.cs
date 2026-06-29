using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TalentFlow.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        // TODO: Implement exception handling logic matching the project's error handling strategy
        base.OnException(context);
    }
}
