using DynamicExamSystem.Domain.Models;
using DynamicExamSystem.infrastructure.Data;
using DynamicExamSystem.infrastructure.repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DynamicExamSystem.infrastructure.repository.Implementations
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly AppDbContext _context;

        public AnswerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Answer> GetByIdAsync(int id)
        {
            return await _context.Answers.FindAsync(id);
        }

        public async Task<IEnumerable<Answer>> GetAllAsync()
        {
            return await _context.Answers.Include(a => a.Question).ToListAsync();
        }

        public async Task<IEnumerable<Answer>> FindAsync(Expression<Func<Answer, bool>> predicate)
        {
            return await _context.Answers.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(Answer answer)
        {
            await _context.Answers.AddAsync(answer);
        }

        public async Task AddRangeAsync(IEnumerable<Answer> answers)
        {
            await _context.Answers.AddRangeAsync(answers);
        }

        public void Update(Answer answer)
        {
            _context.Answers.Update(answer);
        }

        public void Remove(Answer answer)
        {
            _context.Answers.Remove(answer);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
