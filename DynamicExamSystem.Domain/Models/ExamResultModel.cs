namespace DynamicExamSystem.Models
{
    public class ExamResultModel
    {
        public int UserId {  get; set; }
        public int ExamId {  get; set; }

        public byte Score { get; set; }
        public ExamModel Exam { get; set; }

    }
}
