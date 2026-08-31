using MediatR;

public class TenantRegisterCommand : IRequest<AuthResponse>
{
    public string TenantName { get; set; }
    public string Slug { get; set; }
    public string SubscriptionPlan { get; set; }
    public string CompanySize { get; set; }
    public string Industry { get; set; }
    public string? Website { get; set; }
    public string? LinkedIn { get; set; }
    public string? OfficeLocation { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
