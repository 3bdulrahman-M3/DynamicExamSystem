using DynamicExamSystem.Models;

public interface IStudentExamRepository
{
    Task<IEnumerable<StudentExamHestory>> GetStudentHistoryAsync(string userId);
}
