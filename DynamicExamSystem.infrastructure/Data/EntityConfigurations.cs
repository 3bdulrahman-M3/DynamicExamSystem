using DynamicExamSystem.Domain.Models;
using DynamicExamSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DynamicExamSystem.infrastructure.Data
{
    public static class EntityConfigurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable("Subjects");

            // Primary Key
            builder.HasKey(s => s.Id);

            // Properties
            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Relationships
            builder.HasMany(s => s.Exams)
                .WithOne(e => e.Subject)
                .HasForeignKey(e => e.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable("Exams");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties
            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            // Relationships
            builder.HasMany(e => e.Questions)
                .WithOne(q => q.Exam)
                .HasForeignKey(q => q.ExamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Subject)
                .WithMany(s => s.Exams)
                .HasForeignKey(e => e.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            // Properties
            builder.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(50);

            // Relationships
            builder.HasMany(u => u.ExamHistories)
                .WithOne(sh => sh.User)
                .HasForeignKey(sh => sh.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");

            // Primary Key
            builder.HasKey(q => q.Id);

            // Properties
            builder.Property(q => q.Text)
                .IsRequired()
                .HasMaxLength(500);

            // Relationships
            builder.HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(q => q.Exam)
                .WithMany(e => e.Questions)
                .HasForeignKey(q => q.ExamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers");

            // Primary Key
            builder.HasKey(a => a.Id);

            // Properties
            builder.Property(a => a.Text)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.IsCorrect)
                .IsRequired();

            // Relationships
            builder.HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class StudentHistoryConfiguration : IEntityTypeConfiguration<StudentHistory>
    {
        public void Configure(EntityTypeBuilder<StudentHistory> builder)
        {
            builder.ToTable("StudentHistories");

            // Primary Key
            builder.HasKey(sh => sh.Id);

            // Properties
            builder.Property(sh => sh.StartTime)
                .IsRequired();

            builder.Property(sh => sh.EndTime)
                .IsRequired();

            builder.Property(sh => sh.Score)
                .IsRequired();

            builder.Property(sh => sh.FinalScore)
                .IsRequired();

            // Relationships
            builder.HasOne(sh => sh.User)
                .WithMany(u => u.ExamHistories)
                .HasForeignKey(sh => sh.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sh => sh.Exam)
                .WithMany()
                .HasForeignKey(sh => sh.ExamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

}
