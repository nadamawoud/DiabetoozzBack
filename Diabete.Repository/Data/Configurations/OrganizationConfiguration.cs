using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Diabetes.Core.Entities;

namespace Diabetes.Data.Configurations
{
    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.ToTable("Organizations");

            

            builder.Property(o => o.Name).HasMaxLength(200).IsRequired();
            builder.Property(o => o.Email).HasMaxLength(250).IsRequired();
            builder.Property(o => o.PasswordHash).HasMaxLength(500).IsRequired();
            builder.Property(o => o.IsMedicalSyndicate).IsRequired();

            // علاقة Many-to-One مع Admin
            builder.HasOne(o => o.Admin)
                .WithMany(a => a.Organizations)
                .HasForeignKey(o => o.AdminID)
                  .OnDelete(DeleteBehavior.Restrict);

            // علاقة One-to-Many مع Post
            builder.HasMany(o => o.Posts)
                .WithOne(p => p.Organization)
                .HasForeignKey(p => p.OrganizationID)
                  .OnDelete(DeleteBehavior.Restrict);

            // علاقة One-to-Many مع DoctorApproval
            builder.HasMany(o => o.DoctorApprovals)
                .WithOne(da => da.Organization)
                .HasForeignKey(da => da.OrganizationID)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}