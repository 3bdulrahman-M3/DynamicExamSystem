using DynamicExamSystem.Models;
using DynamicExamSystem.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DynamicExamSystem.infrastructure.repository.Interfaces;

public class QuestionRepository : IQuestionRepository
{
    private readonly AppDbContext _context;

    public QuestionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Question> GetByIdAsync(int id)
    {
        return await _context.Questions
            .Include(q => q.Exam).FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<Question> GetByIdWithAnswersAsync(int id)
    {
        return await _context.Questions
            .Include(q => q.Answers)  
            .FirstOrDefaultAsync(q => q.Id == id);  
    }

    public async Task<IEnumerable<Question>> GetAllQuestionsWithAnswersAsync()
    {
        return await _context.Questions
            .Include(q => q.Answers)  
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> FindAsync(Expression<Func<Question, bool>> predicate)
    {
        return await _context.Questions
            .Where(predicate)
            .Include(q => q.Exam)
            .ToListAsync();
    }

    public async Task<Question> AddAsync(Question question)
    {
        await _context.Questions.AddAsync(question);
        await _context.SaveChangesAsync();
        return question;
    }

    public void Update(Question question)
    {
        _context.Questions.Update(question);
    }

    public void Remove(Question question)
    {
        _context.Questions.Remove(question);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
