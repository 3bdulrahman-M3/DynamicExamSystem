using Application.Dtos;
using DynamicExamSystem.infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class ExamResultRepository : IExamResultRepository
{
    private readonly AppDbContext _context;

    public ExamResultRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ExamResultDto>> GetStudentExamResultsAsync(string userId)
    {
        // Fetch the exam histories for the user
        var studentHistories = await _context.StudentHistories
            .Where(sh => sh.UserId == userId)
            .Include(sh => sh.Exam)
            .ThenInclude(e => e.Questions) // Include questions in exam
            .ToListAsync();

        // Map the StudentHistory entities to ExamResultDto
        var results = studentHistories.Select(sh => new ExamResultDto
        {
            ExamId = sh.ExamId,
            TotalQuestions = sh.Exam.Questions.Count,
            CorrectAnswers = sh.Score, // Assuming Score is the number of correct answers
            Score = sh.FinalScore
        }).ToList();

        return results;
    }
}
