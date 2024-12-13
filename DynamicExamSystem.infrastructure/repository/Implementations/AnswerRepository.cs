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

        public async Task AddAsync(Answer answer)
        {
            await _context.Answers.AddAsync(answer);
        }
        public async Task<Answer> GetByIdAsync(int id)
        {
            return await _context.Answers.FindAsync(id);
        }

        public void Delete(Answer answer)
        {
            _context.Answers.Remove(answer);
        }
        public void Update(Answer answer)
        {
            _context.Answers.Update(answer);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


    }

}
