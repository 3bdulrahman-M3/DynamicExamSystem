using DynamicExamSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicExamSystem.Domain.Models
{
    public class SubjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ExamModel> Exams { get; set; }
        public TimeSpan Duration{ get; set; }
    }
}
