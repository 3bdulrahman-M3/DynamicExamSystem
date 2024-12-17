using DynamicExamSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DynamicExamSystem.Domain.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation properties
        public ICollection<Exam> Exams { get; set; } = new List<Exam>();
    }

}

