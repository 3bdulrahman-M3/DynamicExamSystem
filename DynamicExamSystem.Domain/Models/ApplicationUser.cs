using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicExamSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace DynamicExamSystem.infrastructure.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
        public ICollection<StudentHistory> ExamHistories { get; set; } = new List<StudentHistory>();
    }
}
