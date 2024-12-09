using Application.Dtos;
using AutoMapper;
using DynamicExamSystem.Domain.Models;
using DynamicExamSystem.infrastructure.repository.Interfaces;
using DynamicExamSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DynamicExamSystem.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamRepository _examRepository;
        private readonly IMapper _mapper;

        public ExamController(IExamRepository examRepository, IMapper mapper)
        {
            _examRepository = examRepository;
            _mapper = mapper;
        }

        [Authorize(Roles = "Student, Admin")]
        [HttpGet("AllExams")]
        public async Task<ActionResult> GetAllExams()
        {
            var exams = await _examRepository.GetAllAsync();
            var examDtos = _mapper.Map<List<ExamDto>>(exams);

            return Ok(examDtos);
        }

        [Authorize(Roles = "Student, Admin")]
        [HttpGet("ExamsBySubject/{subjectId}")]
        public async Task<ActionResult> GetExamsBySubject(int subjectId)
        {
            var exams = await _examRepository.FindAsync(e => e.SubjectId == subjectId);

            if (!exams.Any())
            {
                return NotFound($"No exams found for Subject ID {subjectId}.");
            }

            var examDtos = _mapper.Map<List<ExamDto>>(exams);
            return Ok(examDtos);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddExam")]
        public async Task<ActionResult> AddExam([FromForm] CreateExamDto dto)
        {
            var newExam = _mapper.Map<Exam>(dto);
            await _examRepository.AddAsync(newExam);
            await _examRepository.SaveChangesAsync();

            return Ok(newExam);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateExam/{id}")]
        public async Task<IActionResult> UpdateExam(int id, [FromForm] CreateExamDto dto)
        {
            var exam = await _examRepository.GetByIdAsync(id);
            if (exam == null)
            {
                return NotFound($"Exam with ID {id} not found.");
            }

            _mapper.Map(dto, exam);
            _examRepository.Update(exam);
            await _examRepository.SaveChangesAsync();

            return Ok(exam);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteExam/{id}")]
        public async Task<IActionResult> DeleteExam(int id)
        {
            var exam = await _examRepository.GetByIdAsync(id);
            if (exam == null)
            {
                return NotFound($"Exam with ID {id} not found.");
            }

            _examRepository.Remove(exam);
            await _examRepository.SaveChangesAsync();

            return Ok($"Exam with ID {id} deleted successfully.");
        }
    }
}
