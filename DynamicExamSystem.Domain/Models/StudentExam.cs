
using DynamicExamSystem.infrastructure.Data;

namespace DynamicExamSystem.Models
{
    public class StudentExam
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public DateTime TakenAt { get; set; }
        public double Score { get; set; }

    }
}
