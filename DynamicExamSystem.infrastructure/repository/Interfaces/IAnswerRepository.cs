using DynamicExamSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DynamicExamSystem.infrastructure.repository.Interfaces
{
    public interface IAnswerRepository
    {
        Task<Answer> GetByIdAsync(int id);
        Task<IEnumerable<Answer>> GetAllAsync();
        Task<IEnumerable<Answer>> FindAsync(Expression<Func<Answer, bool>> predicate);
        Task AddAsync(Answer answer);
        Task AddRangeAsync(IEnumerable<Answer> answers);
        void Update(Answer answer);
        void Remove(Answer answer);
        Task SaveChangesAsync();
    }
}
