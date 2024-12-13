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
            .Include(q => q.Answers) // Optional: Include related answers
            .FirstOrDefaultAsync(q => q.Id == id);
    }
}
