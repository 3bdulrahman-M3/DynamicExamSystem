using DynamicExamSystem.Domain.Models;

namespace DynamicExamSystem.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string CorrectAnswer { get; set; }
        public int ExamId { get; set; }
        public List<Exam> Exam { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
