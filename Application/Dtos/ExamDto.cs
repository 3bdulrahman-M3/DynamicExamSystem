using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class ExamDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SubjectId { get; set; }

    }
    public class CreateExamDto
    {
        public string Title { get; set; }
        public int SubjectId { get; set; }
    }
}
