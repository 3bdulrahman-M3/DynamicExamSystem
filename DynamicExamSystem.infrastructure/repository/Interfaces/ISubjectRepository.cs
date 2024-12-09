using DynamicExamSystem.Domain.Models;
using DynamicExamSystem.Models;
using System.Linq.Expressions;

public interface ISubjectRepository
{
    Task<Subject> GetByIdAsync(int id);
    Task<IEnumerable<Subject>> GetAllAsync();
    Task AddAsync(Subject subject);
    void Update(Subject subject);
    void Remove(Subject subject);
    Task SaveChangesAsync();
}
