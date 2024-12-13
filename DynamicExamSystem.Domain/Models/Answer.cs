using DynamicExamSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicExamSystem.Domain.Models
{
    public class Answer
    {
        public int Id { get; set; } 
        public string Text { get; set; } = string.Empty;

        public int QuestionId { get; set; }

        public Question Question { get; set; } = null!;
        public bool IsCorrect { get; set; } 
    }

}
