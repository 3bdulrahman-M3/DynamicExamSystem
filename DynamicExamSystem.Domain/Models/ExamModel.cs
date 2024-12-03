using DynamicExamSystem.Domain.Models;

namespace DynamicExamSystem.Models
{
    public class ExamModel
    {
        [Key] public int Id { get; set; }
        public string Title { get; set; }
        public int SubjectId { get; set; }
        public SubjectModel Subject { get; set; }
        public ICollection<QuestionModel> Questions { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
