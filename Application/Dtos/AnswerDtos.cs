using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{

    public class AnswerDto
    {
        public string Text { get; set; } = string.Empty;
    }

    public class OptionDto
    {
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }



}
