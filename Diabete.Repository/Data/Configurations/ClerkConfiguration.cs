using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Diabetes.Core.Entities;

namespace Diabetes.Data.Configurations
{
    public class ClerkConfiguration : IEntityTypeConfiguration<Clerk>
    {
        public void Configure(EntityTypeBuilder<Clerk> builder)
        {
            builder.ToTable("Clerks");

          

            builder.Property(c => c.Name).HasMaxLength(200).IsRequired();
            builder.Property(c => c.Email).HasMaxLength(250).IsRequired();
            builder.Property(c => c.BirthDate).IsRequired();
            builder.Property(c => c.Gender).HasMaxLength(10).IsRequired();
            builder.Property(c => c.LicenseCode).HasMaxLength(100).IsRequired();
            builder.Property(c => c.PhoneNumber).HasMaxLength(20).IsRequired();
            builder.Property(c => c.PasswordHash).HasMaxLength(500).IsRequired();

            // علاقة Many-to-One مع Admin
            builder.HasOne(c => c.Admin)
                .WithMany(a => a.Clerks)
                .HasForeignKey(c => c.AdminID);

            // علاقة One-to-Many مع Patient
            builder.HasMany(c => c.Patients)
                .WithOne(p => p.Clerk)
                .HasForeignKey(p => p.ClerkID);
        }
    }
}
