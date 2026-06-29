using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TalentFlow.Persistence.Configurations;

public class JobSkillConfiguration : IEntityTypeConfiguration<TalentFlow.Domain.Entities.RecruitmentModule.JobSkill>
{
    public void Configure(EntityTypeBuilder<TalentFlow.Domain.Entities.RecruitmentModule.JobSkill> builder)
    {
        builder.ToTable("JobSkill");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        // Soft‑delete filter if the entity implements ISoftDelete
        if (typeof(TalentFlow.Domain.Common.ISoftDelete).IsAssignableFrom(typeof(TalentFlow.Domain.Entities.RecruitmentModule.JobSkill)))
        {
            builder.HasQueryFilter(e => !((TalentFlow.Domain.Common.ISoftDelete)e).IsDeleted);
        }
    }
}
