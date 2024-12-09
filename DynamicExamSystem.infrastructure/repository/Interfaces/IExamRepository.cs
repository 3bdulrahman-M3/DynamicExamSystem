using DynamicExamSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DynamicExamSystem.infrastructure.repository.Interfaces
{
    public interface IExamRepository
    {
        Task<Exam> GetByIdAsync(int id);
        Task<IEnumerable<Exam>> GetAllAsync();
        Task<IEnumerable<Exam>> FindAsync(Expression<Func<Exam, bool>> predicate);
        Task AddAsync(Exam exam);
        void Update(Exam exam);
        void Remove(Exam exam);
        Task SaveChangesAsync();
    }
}
