using Application.Dtos;
using DynamicExamSystem.Models;
using Microsoft.AspNetCore.Mvc;

public interface IExamResultRepository
{
    Task<List<ExamResultDto>> GetStudentExamResultsAsync(string userId);
    Task<(IEnumerable<StudentHistoryDTO> Histories, int TotalCount)> GetAllStudentHistoryAsync(int pageNumber, int pageSize);
    Task<(IEnumerable<StudentHistoryDTO> Histories, int TotalCount)> GetStudentHistoryByIdAsync(string userId, int pageNumber, int pageSize);


}

