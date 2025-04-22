using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Diabetes.Core.Entities;

namespace Diabetes.Data.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("Admins");

            builder.Property(a => a.Email).HasMaxLength(100).IsRequired();
            builder.Property(a => a.PasswordHash).HasMaxLength(256).IsRequired();

            // تكوين علاقة 1-to-1
            builder.HasOne(a => a.Manager)
                   .WithOne(m => m.Admin)
                   .HasForeignKey<Admin>(a => a.ManagerID);

            // 1-to-Many with Clerk
            builder.HasMany(a => a.Clerks)
                   .WithOne(c => c.Admin)
                   .HasForeignKey(c => c.AdminID);

            // 1-to-Many with Doctor
            builder.HasMany(a => a.Doctors)
                   .WithOne(d => d.Admin)
                   .HasForeignKey(d => d.AdminID);

            // 1-to-Many with Patient
            builder.HasMany(a => a.Patients)
                   .WithOne(p => p.Admin)
                   .HasForeignKey(p => p.AdminID);

            // 1-to-Many with Organization
            builder.HasMany(a => a.Organizations)
                   .WithOne(o => o.Admin)
                   .HasForeignKey(o => o.AdminID);

            // 1-to-Many with CasualUser
            builder.HasMany(a => a.CasualUsers)
                   .WithOne(c => c.Admin)
                   .HasForeignKey(c => c.AdminID);

            // 1-to-Many with ChatbotQuestionDoctor
            builder.HasMany(a => a.ChatbotQuestionDoctors)
                   .WithOne(c => c.Admin)
                   .HasForeignKey(c => c.AdminID);

            // 1-to-Many with ChatbotQuestionCasualUser
            builder.HasMany(a => a.ChatbotQuestionCasualUsers)
                   .WithOne(c => c.Admin)
                   .HasForeignKey(c => c.AdminID);
        }
    }
}