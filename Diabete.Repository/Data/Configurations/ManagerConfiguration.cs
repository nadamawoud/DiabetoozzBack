using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Diabetes.Core.Entities;

namespace Diabetes.Data.Configurations
{
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.ToTable("Managers");

            

            builder.Property(m => m.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(m => m.PasswordHash)
                .HasMaxLength(256)
                .IsRequired();

            // علاقة 1-to-1 مع Admin
            builder.HasOne(m => m.Admin)
                .WithOne(a => a.Manager)
                .HasForeignKey<Manager>(m => m.AdminID);
        }
    }
}