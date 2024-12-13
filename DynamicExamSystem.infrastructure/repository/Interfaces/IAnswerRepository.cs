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
        Task AddAsync(Answer answer);
        Task<Answer> GetByIdAsync(int id);
        void Update(Answer answer);
        void Delete(Answer answer);
        Task SaveChangesAsync();
    }
}
