using DynamicExamSystem.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace DynamicExamSystem.Models
{
    public class Exam
    {
        [Key] public int Id { get; set; }
        public string Title { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject{ get; set; }
        public List<Question> Questions { get; set; }
    }
}
