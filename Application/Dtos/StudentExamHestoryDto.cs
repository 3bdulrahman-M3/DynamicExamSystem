using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class StudentExamHistoryDto
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public DateTime TakenAt { get; set; }
        public double Score { get; set; }
    }


}
