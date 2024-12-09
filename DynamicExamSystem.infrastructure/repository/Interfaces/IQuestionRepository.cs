using DynamicExamSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DynamicExamSystem.infrastructure.repository.Interfaces
{
    public interface IQuestionRepository
    {
        Task<Question> GetByIdAsync(int id);
        Task<Question> GetByIdWithAnswersAsync(int id);
        Task<IEnumerable<Question>> GetAllQuestionsWithAnswersAsync();
        Task<IEnumerable<Question>> FindAsync(Expression<Func<Question, bool>> predicate);
        Task<Question> AddAsync(Question question);
        void Update(Question question);
        void Remove(Question question);
        Task SaveChangesAsync();
    }
}
