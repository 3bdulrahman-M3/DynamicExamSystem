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
        var studentHistories = await _context.StudentHistories
            .Where(sh => sh.UserId == userId)
            .Include(sh => sh.Exam)
            .ThenInclude(e => e.Questions)
            .ToListAsync();

        var results = studentHistories.Select(sh => new ExamResultDto
        {
            ExamId = sh.ExamId,
            Title = sh.Exam.Title,
            TotalQuestions = sh.Exam.Questions.Count,
            CorrectAnswers = sh.Score, 
            Score = sh.FinalScore
        }).ToList();

        return results;
    }

    public async Task<(IEnumerable<StudentHistoryDTO> Histories, int TotalCount)> GetAllStudentHistoryAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.StudentHistories.CountAsync();

        var histories = await _context.StudentHistories
            .Include(sh => sh.User)
            .Include(sh => sh.Exam)
            .Select(sh => new StudentHistoryDTO
            {
                UserName = sh.User.UserName,
                ExamTitle = sh.Exam.Title,
                StartTime = sh.StartTime,
                EndTime = sh.EndTime,
                TimeTaken = sh.TimeTaken,
                Score = sh.Score,
                FinalScore = sh.FinalScore
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (histories, totalCount);
    }

    public async Task<(IEnumerable<StudentHistoryDTO> Histories, int TotalCount)> GetStudentHistoryByIdAsync(string userId, int pageNumber, int pageSize)
    {
        var totalCount = await _context.StudentHistories
            .Where(history => history.UserId == userId)
            .CountAsync();

        var histories = await _context.StudentHistories
            .Where(history => history.UserId == userId)
            .Select(history => new StudentHistoryDTO
            {
                UserName = history.User.UserName,
                ExamTitle = history.Exam.Title,
                StartTime = history.StartTime,
                EndTime = history.EndTime,
                TimeTaken = history.EndTime - history.StartTime,
                Score = history.Score,
                FinalScore = history.FinalScore
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (histories, totalCount);
    }


}
