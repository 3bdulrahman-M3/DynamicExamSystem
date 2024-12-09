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

    public async Task<Exam> GetByIdAsync(int id)
    {
        return await _context.Exams.Include(e => e.Subject).FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Exam>> GetAllAsync()
    {
        return await _context.Exams.Include(e => e.Subject).ToListAsync();
    }

    public async Task<IEnumerable<Exam>> FindAsync(Expression<Func<Exam, bool>> predicate)
    {
        return await _context.Exams.Where(predicate).Include(e => e.Subject).ToListAsync();
    }

    public async Task AddAsync(Exam exam)
    {
        await _context.Exams.AddAsync(exam);
    }

    public void Update(Exam exam)
    {
        _context.Exams.Update(exam);
    }

    public void Remove(Exam exam)
    {
        _context.Exams.Remove(exam);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
