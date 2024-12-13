using DynamicExamSystem.Domain.Models;

namespace DynamicExamSystem.Models
{
   
        public class Question
        {
            public int Id { get; set; } 
            public string Text { get; set; } = string.Empty;

            public int ExamId { get; set; }

            public Exam Exam { get; set; } = null!;
            public ICollection<Answer> Answers { get; set; } = new List<Answer>();
        }

    
}
