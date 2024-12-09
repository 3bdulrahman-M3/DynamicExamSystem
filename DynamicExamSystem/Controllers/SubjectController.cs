using Application.Dtos;
using DynamicExamSystem.Domain.Models;
using DynamicExamSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicExamSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectController(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        [Authorize(Roles = "Student, Admin")]
        [HttpGet("AllSubjects")]
        public async Task<ActionResult> GetSubjects()
        {
            var subjects = await _subjectRepository.GetAllAsync();
            return Ok(subjects);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateSubject([FromForm] SubjectDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data.");
            }

            var subject = new Subject
            {
                Name = dto.Name
            };

            await _subjectRepository.AddAsync(subject);
            await _subjectRepository.SaveChangesAsync();

            return Ok(subject);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSubject(int id, [FromForm] SubjectDto dto)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);
            if (subject == null)
            {
                return NotFound("Subject not found.");
            }

            subject.Name = dto.Name;
            _subjectRepository.Update(subject);
            await _subjectRepository.SaveChangesAsync();

            return Ok(subject);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubject(int id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);
            if (subject == null)
            {
                return NotFound("Subject not found.");
            }

            _subjectRepository.Remove(subject);
            await _subjectRepository.SaveChangesAsync();

            return Ok(subject);
        }
    }
}
