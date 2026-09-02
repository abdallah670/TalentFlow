namespace TalentFlow.Application.Contracts.Infra  
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        Guid TenantId { get; }

    }
}