namespace TalentFlow.Application.Exceptions;

/// <summary>
/// Exception thrown when a user attempts to access or modify a resource they don't have permission for.
/// </summary>
public class UnauthorizedAccessException : ApplicationException
{
    public UnauthorizedAccessException(string message) : base(message)
    {
    }

    public UnauthorizedAccessException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Creates an UnauthorizedAccessException for when a user tries to access another user's resource.
    /// </summary>
    /// <param name="resourceName">The name of the resource (e.g., "Clothing item")</param>
    /// <param name="userId">The ID of the user attempting access</param>
    /// <returns>A new UnauthorizedAccessException instance</returns>
    public static UnauthorizedAccessException ForResource(string resourceName, string userId)
    {
        return new UnauthorizedAccessException($"User '{userId}' is not authorized to access this {resourceName.ToLower()}");
    }
}
