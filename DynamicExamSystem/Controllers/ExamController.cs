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
using AutoMapper;

namespace DynamicExamSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ExamController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("AllExams")]
        public async Task<ActionResult> GetAllExams()
        {
            var exams = await _context.Exams
                .Include(e => e.Subject)
                .ToListAsync();

            var examDtos = _mapper.Map<List<ExamDto>>(exams);

            return Ok(examDtos);
        }


        [HttpGet("ExamsBySubject/{subjectId}")]
        public async Task<ActionResult> GetExamsBySubject(int subjectId)
        {
            var exams = await _context.Exams
                .Where(e => e.SubjectId == subjectId)
                .ToListAsync();

            if (exams == null || exams.Count == 0)
            {
                return NotFound($"No exams found for SubjectId {subjectId}.");
            }

            var examDtos = _mapper.Map<List<ExamDto>>(exams);

            return Ok(examDtos);
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

            _mapper.Map(dto, exam);

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
            return Ok(exam);
        }
    }
}
