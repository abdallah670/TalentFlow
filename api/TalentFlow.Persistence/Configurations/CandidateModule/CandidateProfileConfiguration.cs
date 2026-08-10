using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentFlow.Domain.Entities.CandidateModule;

namespace TalentFlow.Persistence.Configurations.CandidateModule
{
    public class CandidateProfileConfiguration : IEntityTypeConfiguration<CandidateProfile>
    {
        public void Configure(EntityTypeBuilder<CandidateProfile> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PhoneNumber).HasMaxLength(30);
            builder.Property(x => x.CurrentJobTitle).HasMaxLength(150);
            builder.Property(x => x.CurrentCompany).HasMaxLength(150);
            builder.Property(x => x.LinkedInUrl).HasMaxLength(500);
            builder.Property(x => x.PortfolioUrl).HasMaxLength(500);
            builder.Property(x => x.ResumeUrl).HasMaxLength(500);
            builder.Property(x => x.ResumeFileName).HasMaxLength(255);
            builder.Property(x => x.PreferredLocation).HasMaxLength(150);
            builder.Property(x => x.Currency).HasMaxLength(10);

            builder.Property(x => x.MinSalaryExpectation).HasColumnType("decimal(18,2)");
            builder.Property(x => x.MaxSalaryExpectation).HasColumnType("decimal(18,2)");

            // One-to-One مع User
            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<CandidateProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.UserId).IsUnique();

         
        }
    }
}