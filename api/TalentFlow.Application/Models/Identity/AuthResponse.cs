public class AuthResponse
{
    public bool IsAuthenticated { get; set; }

    public string? Message { get; set; }

    public string Id { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public List<string> Roles { get; set; } = new();

    public string Token { get; set; } = string.Empty;

    public DateTime TokenExpiration { get; set; }

    public string RefreshToken { get; set; } = string.Empty;

    public DateTime RefreshTokenExpiration { get; set; }
}