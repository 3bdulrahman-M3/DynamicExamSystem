using DynamicExamSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DynamicExamSystem.infrastructure.Data;

public class ExamDbContext : IdentityDbContext<ApplicationUser>
{
    public ExamDbContext(DbContextOptions<ExamDbContext> options) : base(options)
    {}
    public DbSet <ExamModel> Exams{ get; set; }
    public DbSet<ExamResultModel> ExamResults { get; set; }
    public DbSet<QuestionModel> Questions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ExamResultModel>()
            .HasKey(e => new { e.ExamId, e.UserId }); 
    }




}
