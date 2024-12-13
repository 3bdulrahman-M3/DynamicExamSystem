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
        public string Title { get; set; } = string.Empty;
        public int SubjectId { get; set; }
    }

}
