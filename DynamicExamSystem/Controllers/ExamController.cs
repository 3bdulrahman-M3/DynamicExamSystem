using DynamicExamSystem.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DynamicExamSystem.infrastructure.Data;
using Application.Dtos;
using System.Collections.Immutable;
using DynamicExamSystem.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DynamicExamSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExamController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("AllExams")]
        public async Task<ActionResult> GetAllExams()
        {
            var exams = await _context.Exams
                .Include(e => e.Subject) 
                .Select(e => new ExamDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    SubjectId = e.SubjectId,
                })
                .ToListAsync();

            return Ok(exams);
        }

        [HttpGet("ExamsBySubject/{subjectId}")]
        public async Task<ActionResult> GetExamsBySubject(int subjectId)
        {
            var exams = await _context.Exams
                .Where(e => e.SubjectId == subjectId)
                .Select(e => new ExamDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    SubjectId = e.SubjectId,
                })
                .ToListAsync();

            if (exams == null || exams.Count == 0)
            {
                return NotFound($"No exams found for SubjectId {subjectId}.");
            }

            return Ok(exams);
        }

        [HttpPost("AddExam")]
        public async Task<ActionResult> AddExam([FromForm] CreateExamDto dto)
        {
            var subject = await _context.Subjects.FindAsync(dto.SubjectId);
            if (subject == null)
            {
                return NotFound($"Subject with ID {dto.SubjectId} not found.");
            }

            var newExam = new Exam
            {
                SubjectId = dto.SubjectId,
                Title = dto.Title
            };

            _context.Exams.Add(newExam);
            await _context.SaveChangesAsync();

            return Ok(newExam);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExam(int Id , [FromForm] CreateExamDto dto)
        {
            var exam = await _context.Exams.FindAsync(Id);
            if (exam == null)
            {
                return NotFound($"Exam with ID {Id} not found.");
            }

            exam.SubjectId = dto.SubjectId;
            exam.Title = dto.Title;

            _context.Exams.Update(exam);
            await _context.SaveChangesAsync();
            return Ok(exam);
        }



        [HttpDelete]
        public async Task<IActionResult> DelelteExam([FromForm]int id)
        {
         var exam =  await _context.Exams.FindAsync(id);
            if (exam == null) {
                return NotFound($"Exam with ID {id} not found.");
            }
            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();
            return Ok($"The {exam.Title} Deleted");
        }






    }
}
