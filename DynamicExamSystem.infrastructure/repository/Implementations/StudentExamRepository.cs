using DynamicExamSystem.infrastructure.Data;
using DynamicExamSystem.Models;
using Microsoft.EntityFrameworkCore;

public class StudentExamRepository : IStudentExamRepository
{
    private readonly AppDbContext _context;

    public StudentExamRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StudentExamHestory>> GetStudentHistoryAsync(string userId)
    {
        return await _context.StudentExams
            .Include(se => se.Exam)
            .Where(se => se.UserId == userId)
            .OrderByDescending(se => se.TakenAt)
            .ToListAsync();
    }
}
