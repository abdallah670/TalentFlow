public class AuthResponse
{
    public bool IsAuthenticated { get; set; }

    public string? Message { get; set; }

    public string Id { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public List<string> Roles { get; set; } = new();
    public int CurrentStep { get; set; }

    public bool OnboardingCompleted { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime TokenExpiration { get; set; }

    public string RefreshToken { get; set; } = string.Empty;

    public DateTime RefreshTokenExpiration { get; set; }
}
public class TenantOptionDto
{
    public string TenantId { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}