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

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetAllSubjects()
        {
            var subjects = await _subjectRepository.GetAllSubjectsAsync();
            return Ok(subjects);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectDto>> GetSubjectById(int id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);
            if (subject == null)
            {
                return NotFound($"Subject with ID {id} not found.");
            }
            return Ok(subject);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Addsubject")]
        public async Task<ActionResult<SubjectDto>> CreateSubject([FromForm] SubjectCreateDto subjectCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            var subject = new Subject
            {
                Name = subjectCreateDto.Name
            };

            var createdSubject = await _subjectRepository.CreateSubjectAsync(subject);

            return CreatedAtAction(nameof(GetSubjectById), new { id = createdSubject.Id }, createdSubject);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSubject(int id, [FromForm] SubjectCreateDto subjectDto)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);
            if (subject == null)
            {
                return NotFound("Subject not found.");
            }
            subject.Name = subjectDto.Name;
            _subjectRepository.Update(subject);
            await _subjectRepository.SaveChangesAsync();
            return Ok(subject);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var result = await _subjectRepository.DeleteSubjectAsync(id);
            if (!result)
            {
                return NotFound($"Subject with ID {id} not found.");
            }

            return NoContent();
        }
    }

}
