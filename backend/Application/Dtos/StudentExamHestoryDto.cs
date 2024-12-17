using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class AnswerSubmissionDto
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
    }
    public class ExamResultDto
    {
        public int ExamId { get; set; }
        public string Title { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int Score { get; set; }
    }

    public class StudentHistoryDTO
    {
        public string UserName { get; set; }
        public string ExamTitle { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan TimeTaken { get; set; }
        public int Score { get; set; }
        public int FinalScore { get; set; }
    }





}
