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
}
