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

            // تكوين الخصائص الأساسية
            builder.Property(cu => cu.Username).HasMaxLength(100).IsRequired();
            builder.Property(cu => cu.Email).HasMaxLength(250).IsRequired();
            builder.Property(cu => cu.Gender).HasMaxLength(10).IsRequired();
            builder.Property(cu => cu.BirthDate).IsRequired();
            builder.Property(cu => cu.PasswordHash).HasMaxLength(500).IsRequired();
            builder.Property(cu => cu.PhoneNumber).HasMaxLength(20).IsRequired();

            // تكوين علاقة Admin مع تحديد أنها اختيارية
            builder.HasOne(cu => cu.Admin)
                .WithMany(a => a.CasualUsers)
                .HasForeignKey(cu => cu.AdminID)
                .OnDelete(DeleteBehavior.SetNull) // أو DeleteBehavior.NoAction حسب احتياجاتك
                .IsRequired(false); // هذا السطر يجعل العلاقة اختيارية

            // تكوين القيمة الافتراضية لـ AdminID
            builder.Property(cu => cu.AdminID)
                .HasDefaultValue(1); // تأكد من وجود إداري بهذا ID

            // تكوين العلاقات الأخرى
            builder.HasMany(cu => cu.BloodSugars)
                .WithOne(bs => bs.CasualUser)
                .HasForeignKey(bs => bs.CasualUserID);

            builder.HasMany(cu => cu.Alarms)
                .WithOne(a => a.CasualUser)
                .HasForeignKey(a => a.CasualUserID);

            builder.HasMany(cu => cu.ChatbotResultCasualUsers)
                .WithOne(crcu => crcu.CasualUser)
                .HasForeignKey(crcu => crcu.CasualUserID);

            builder.HasMany(cu => cu.ChatbotAnswerCasualUsers)
                .WithOne(cacu => cacu.CasualUser)
                .HasForeignKey(cacu => cacu.CasualUserID);
        }
    }
}
