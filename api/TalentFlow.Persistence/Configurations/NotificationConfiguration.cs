using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TalentFlow.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<TalentFlow.Domain.Entities.NotificationModule.Notification>
{
    public void Configure(EntityTypeBuilder<TalentFlow.Domain.Entities.NotificationModule.Notification> builder)
    {
        builder.ToTable("Notification");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        // Soft‑delete filter if the entity implements ISoftDelete
        if (typeof(TalentFlow.Domain.Common.ISoftDelete).IsAssignableFrom(typeof(TalentFlow.Domain.Entities.NotificationModule.Notification)))
        {
            builder.HasQueryFilter(e => !((TalentFlow.Domain.Common.ISoftDelete)e).IsDeleted);
        }
    }
}
