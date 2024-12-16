using Application.Dtos;
using DynamicExamSystem.Domain.Models;
using DynamicExamSystem.infrastructure.Data;
using DynamicExamSystem.infrastructure.repository.Interfaces;
using DynamicExamSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

public class ExamRepository : IExamRepository
{
    private readonly AppDbContext _context;

    public ExamRepository(AppDbContext context)
    {
        _context = context;
    }

    //
    public async Task<Exam> AddAsync(Exam exam)
    {
        await _context.Exams.AddAsync(exam);
        return exam;
    }
    //
    public async Task<Exam> GetExamByIdAsync(int examId)
    {
        return await _context.Exams
            .Include(exam => exam.Questions).ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(exam => exam.Id == examId);
    }

    //
    public async Task<IEnumerable<Exam>> GetExamsBySubjectIdAsync(int subjectId)
    {
        return await _context.Exams
            .Where(e => e.SubjectId == subjectId)
            .Include(e => e.Subject)
            .ToListAsync();
    }
    //
    public async Task AddQuestionAsync(Question question)
    {
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
    }

    public async Task AddQuestionToExamAsync(int examId, Question question)
    {
        var exam = await _context.Exams
            .Include(exam => exam.Questions)
            .FirstOrDefaultAsync(exam => exam.Id == examId);

        if (exam == null)
        {
            throw new KeyNotFoundException($"Exam with ID {examId} not found.");
        }
        question.ExamId = examId;
        exam.Questions.Add(question);
        await _context.SaveChangesAsync();
    }

    public async Task Remove(Question question)
    {
        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
    }

    // Get a question by its ID
    public async Task<Question> GetQuestionByIdAsync(int questionId)
    {
        return await _context.Questions
            .FirstOrDefaultAsync(q => q.Id == questionId);
    }

    public void Update(Exam exam)
    {
        _context.Exams.Update(exam);
    }

    public void Delete(Exam exam)
    {
        _context.Exams.Remove(exam);
    }
    //
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Exam?> GetExamWithQuestionsAsync(int examId)
    {
        return await _context.Exams
            .Include(e => e.Questions)
                .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(e => e.Id == examId);
    }

    public async Task<List<Answer>> GetAnswersByIdsAsync(List<int> answerIds)
    {
        return await _context.Answers
            .Where(a => answerIds.Contains(a.Id))
            .ToListAsync();
    }

    public async Task<ExamResultDto> EvaluateExamAsync(int examId, string userId, List<AnswerSubmissionDto> answers)
    {
        var exam = await GetExamWithQuestionsAsync(examId);
        if (exam == null)
            throw new KeyNotFoundException($"Exam with ID {examId} not found.");


        var submittedAnswerIds = answers.Select(a => a.AnswerId).ToList();
        var submittedAnswers = await GetAnswersByIdsAsync(submittedAnswerIds);


        int correctAnswers = submittedAnswers.Count(a => a.IsCorrect);
        int totalQuestions = exam.Questions.Count;
        int score = (int)((double)correctAnswers / totalQuestions * 100);


        var history = new StudentHistory
        {
            UserId = userId,
            ExamId = examId,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow,
            Score = correctAnswers,
            FinalScore = score
        };

        await _context.StudentHistories.AddAsync(history);
        
        await _context.SaveChangesAsync();

        return new ExamResultDto
        {
            ExamId = examId,
            TotalQuestions = totalQuestions,
            CorrectAnswers = correctAnswers,
            Score = score
        };
    }
}
