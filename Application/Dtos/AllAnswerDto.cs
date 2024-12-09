using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class CreateAnswerDto
    {
        public int QuestionId { get; set; }
        public List<OptionDto> Options { get; set; }

        public class OptionDto
        {
            public string Option { get; set; }
            public bool IsCorrect { get; set; }
        }
    }

    public class OptionsDto
    {
        public int Id { get; set; }
        public string Option { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
    }


}
