using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TalentFlow.Api.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var stopwatch = Stopwatch.StartNew();
            
            // Log the incoming request
            var request = httpContext.Request;
            _logger.LogInformation(
                "HTTP {Method} {Path}{QueryString} started at {Timestamp}",
                request.Method,
                request.Path,
                request.QueryString,
                DateTime.UtcNow);

            try
            {
                await _next(httpContext);
            }
            finally
            {
                stopwatch.Stop();
                
                var response = httpContext.Response;
                
                _logger.LogInformation(
                    "HTTP {Method} {Path}{QueryString} completed with {StatusCode} in {ElapsedMilliseconds}ms",
                    request.Method,
                    request.Path,
                    request.QueryString,
                    response.StatusCode,
                    stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
