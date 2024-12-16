using Application.Dtos;
using DynamicExamSystem.Models;

public interface IExamResultRepository
{
    Task<List<ExamResultDto>> GetStudentExamResultsAsync(string userId);
}

