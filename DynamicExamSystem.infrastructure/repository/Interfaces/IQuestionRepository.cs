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
    }
}
