
using DynamicExamSystem.infrastructure.Data;

namespace DynamicExamSystem.Models
{
    public class StudentHistory
    {
        public int Id { get; set; } 

        public string UserId { get; set; } = string.Empty; 
        public int ExamId { get; set; }

        public Exam Exam { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!; 

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeSpan TimeTaken => EndTime - StartTime;
        public int Score { get; set; }
        public int FinalScore { get; set; } 
    }

}
