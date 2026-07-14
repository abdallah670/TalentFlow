namespace TalentFlow.Application.Interfaces
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        Guid TenantId { get; }

    }
}