using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Diabetes.Core.Entities;

namespace Diabetes.Data.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctors");

            

            builder.Property(d => d.DoctorSpecialization).HasMaxLength(100).IsRequired();
            builder.Property(d => d.Name).HasMaxLength(200).IsRequired();
            builder.Property(d => d.BirthDate).IsRequired();
            builder.Property(d => d.Gender).HasMaxLength(10).IsRequired();
            builder.Property(d => d.PhoneNumber).HasMaxLength(20).IsRequired();
            builder.Property(d => d.MedicalSyndicateCardNumber).HasMaxLength(100).IsRequired();
            builder.Property(d => d.Email).HasMaxLength(250).IsRequired();
            builder.Property(d => d.PasswordHash).HasMaxLength(500).IsRequired();

            // علاقة Many-to-One مع Admin
            builder.HasOne(d => d.Admin)
                .WithMany(a => a.Doctors)
                .HasForeignKey(d => d.AdminID);

            // علاقة One-to-One مع DoctorApproval
            builder.HasOne(d => d.DoctorApproval)
                .WithOne(da => da.Doctor)
                .HasForeignKey<DoctorApproval>(da => da.DoctorID);

                           

            // علاقة One-to-Many مع MedicalHistory
            builder.HasMany(d => d.MedicalHistories)
                .WithOne(mh => mh.Doctor)
                .HasForeignKey(mh => mh.DoctorID);

            // علاقة One-to-Many مع SuggestedFood
            builder.HasMany(d => d.SuggestedFoods)
                .WithOne(sf => sf.Doctor)
                .HasForeignKey(sf => sf.DoctorID);

            // علاقة One-to-Many مع ChatbotAnswerDoctor
            builder.HasMany(d => d.ChatbotAnswerDoctors)
                .WithOne(cad => cad.Doctor)
                .HasForeignKey(cad => cad.DoctorID);
        }
    }
}