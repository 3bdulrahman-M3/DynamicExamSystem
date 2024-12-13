using Application.Dtos;
using DynamicExamSystem.Models;
using Microsoft.AspNetCore.Mvc;
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
        Task<Exam> AddAsync(Exam exam);
        Task<Exam> GetExamByIdAsync(int examId);
        Task<IEnumerable<Exam>> GetExamsBySubjectIdAsync(int subjectId);
        Task<Question> GetQuestionByIdAsync(int questionId);
        Task Remove(Question question);
        Task SaveChangesAsync();
        void Update(Exam exam);
        void Delete(Exam exam);
    }
}
