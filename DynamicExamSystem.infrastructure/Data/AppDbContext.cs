using DynamicExamSystem.Domain.Models;
using DynamicExamSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DynamicExamSystem.infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public readonly object Subject;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}
    public DbSet <Exam> Exams{ get; set; }
    public DbSet<StudentExam> StudentExams { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Answer> Answers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<StudentExam>()
            .HasKey(e => new { e.ExamId, e.UserId }); 

    }




}
