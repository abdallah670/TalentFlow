using MediatR;
using TalentFlow.Application.Responses;

public class TenantRegisterCommand : IRequest<BaseCommandResponse<AuthResponse>>
{
    public string TenantName { get; set; } = default!;
    public string? Slug { get; set; }
    public string SubscriptionPlan { get; set; } = default!;
    public string CompanySize { get; set; } = default!;
    public string Industry { get; set; } = default!;
    public string? Website { get; set; }
    public string? LinkedIn { get; set; }
    public string? OfficeLocation { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? UserName { get; set; }
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}