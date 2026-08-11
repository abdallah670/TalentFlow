using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentFlow.Domain.Entities.CandidateModule;

namespace TalentFlow.Persistence.Configurations.CandidateModule
{
    public class CandidateProfileSkillConfiguration : IEntityTypeConfiguration<CandidateProfileSkill>
    {
        public void Configure(EntityTypeBuilder<CandidateProfileSkill> builder)
        {
            builder.HasOne(x => x.CandidateProfile)
                .WithMany(x => x.Skills)
                .HasForeignKey(x => x.CandidateProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Skill)
                .WithMany()
                .HasForeignKey(x => x.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.CandidateProfileId, x.SkillId }).IsUnique();
        }
    }
}