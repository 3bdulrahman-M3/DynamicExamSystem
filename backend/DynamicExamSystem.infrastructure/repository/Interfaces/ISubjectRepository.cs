using Application.Dtos;
using DynamicExamSystem.Domain.Models;
using DynamicExamSystem.Models;
using System.Linq.Expressions;

public interface ISubjectRepository
{
    Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync();
    Task<Subject> GetByIdAsync(int id);
    void Update(Subject subject);
    Task SaveChangesAsync();
    Task<SubjectDto> CreateSubjectAsync(Subject subject);
    Task<bool> DeleteSubjectAsync(int id);
}
