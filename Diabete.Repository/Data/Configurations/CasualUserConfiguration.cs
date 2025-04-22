using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Diabetes.Core.Entities;

namespace Diabetes.Data.Configurations
{
    public class CasualUserConfiguration : IEntityTypeConfiguration<CasualUser>
    {
        public void Configure(EntityTypeBuilder<CasualUser> builder)
        {
            builder.ToTable("CasualUsers");

            

            builder.Property(cu => cu.Username).HasMaxLength(100).IsRequired();
            builder.Property(cu => cu.Email).HasMaxLength(250).IsRequired();
            builder.Property(cu => cu.Gender).HasMaxLength(10).IsRequired();
            builder.Property(cu => cu.BirthDate).IsRequired();
            builder.Property(cu => cu.PasswordHash).HasMaxLength(500).IsRequired();
            builder.Property(cu => cu.PhoneNumber).HasMaxLength(20).IsRequired();

            // علاقة Many-to-One مع Admin
            builder.HasOne(cu => cu.Admin)
                .WithMany(a => a.CasualUsers)
                .HasForeignKey(cu => cu.AdminID);

            // علاقة One-to-Many مع BloodSugar
            builder.HasMany(cu => cu.BloodSugars)
                .WithOne(bs => bs.CasualUser)
                .HasForeignKey(bs => bs.CasualUserID);

            // علاقة One-to-Many مع Alarm
            builder.HasMany(cu => cu.Alarms)
                .WithOne(a => a.CasualUser)
                .HasForeignKey(a => a.CasualUserID);

            // علاقة One-to-Many مع ChatbotResultCasualUser
            builder.HasMany(cu => cu.ChatbotResultCasualUsers)
                .WithOne(crcu => crcu.CasualUser)
                .HasForeignKey(crcu => crcu.CasualUserID);

            // علاقة One-to-Many مع ChatbotAnswerCasualUser
            builder.HasMany(cu => cu.ChatbotAnswerCasualUsers)
                .WithOne(cacu => cacu.CasualUser)
                .HasForeignKey(cacu => cacu.CasualUserID);
        }
    }
}
