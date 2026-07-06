using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Domain.Entities.NotificationModule;

namespace TalentFlow.Persistence.Repositories;

public class NotificationTemplateRepository : GenericRepository<NotificationTemplate>, INotificationTemplateRepository
{
    public NotificationTemplateRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
