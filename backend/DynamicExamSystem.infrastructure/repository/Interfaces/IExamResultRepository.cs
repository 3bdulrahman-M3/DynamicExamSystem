using Application.Dtos;
using DynamicExamSystem.Models;
using Microsoft.AspNetCore.Mvc;

public interface IExamResultRepository
{
    Task<List<ExamResultDto>> GetStudentExamResultsAsync(string userId);
    Task<IEnumerable<StudentHistoryDTO>> GetAllStudentHistoryAsync();
    Task<IEnumerable<StudentHistoryDTO>> GetStudentHistoryByIdAsync(string studentId);

}

