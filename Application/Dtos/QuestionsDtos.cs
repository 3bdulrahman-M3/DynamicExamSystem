using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Dtos.CreateAnswerDto;

namespace Application.Dtos
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<OptionDto> Options { get; set; }
        public int ExamId { get; set; }
    }

    public class CreateQuestionDto
    {
        public string Text { get; set; }
        public int ExamId { get; set; }
    }
   
}
