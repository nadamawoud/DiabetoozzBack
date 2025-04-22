using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Diabetes.Core.Entities;

namespace Diabetes.Data.Configurations
{
    public class ChatbotAnswerDoctorConfiguration : IEntityTypeConfiguration<ChatbotAnswerDoctor>
    {
        public void Configure(EntityTypeBuilder<ChatbotAnswerDoctor> builder)
        {
            builder.ToTable("ChatbotAnswerDoctors");

            

            builder.Property(cad => cad.AnswerText).IsRequired();
            builder.Property(cad => cad.AnswerDate).IsRequired();

            // علاقة One-to-One مع ChatbotQuestionDoctor
            builder.HasOne(cad => cad.ChatbotQuestionDoctor)
                .WithOne(cqd => cqd.ChatbotAnswerDoctor)
                .HasForeignKey<ChatbotAnswerDoctor>(cad => cad.ChatbotQuestionDoctorID)
                .OnDelete(DeleteBehavior.NoAction);

            // علاقة Many-to-One مع Doctor (تبقى كما هي)
            builder.HasOne(cad => cad.Doctor)
                .WithMany(d => d.ChatbotAnswerDoctors)
                .HasForeignKey(cad => cad.DoctorID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
