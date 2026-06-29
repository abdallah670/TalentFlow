using Hangfire.Dashboard;

namespace TalentFlow.Api.Middleware;

/// <summary>
/// Authorization filter for Hangfire Dashboard
/// In production, you should restrict this to admin users only
/// </summary>
public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // Allow access in development
        var httpContext = context.GetHttpContext();
        if (httpContext.Request.Host.Host.Contains("localhost"))
        {
            return true;
        }

        // In production, check if user is authenticated and is an admin
        // For now, allowing all authenticated users - customize as needed
        return httpContext.User.Identity?.IsAuthenticated ?? false;
    }
}
