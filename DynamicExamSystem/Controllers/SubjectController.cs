using Application.Dtos;
using DynamicExamSystem.Domain.Models;
using DynamicExamSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicExamSystem.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectController(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        // GET: api/Subject
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetAllSubjects()
        {
            var subjects = await _subjectRepository.GetAllSubjectsAsync();
            return Ok(subjects);
        }

        // GET: api/Subject/{id}
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

        // POST: api/Subject
        [HttpPost]
        public async Task<ActionResult<SubjectDto>> CreateSubject([FromBody] SubjectCreateDto subjectCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create a new Subject object with the provided name.
            var subject = new Subject
            {
                Name = subjectCreateDto.Name
            };

            var createdSubject = await _subjectRepository.CreateSubjectAsync(subject);

            return CreatedAtAction(nameof(GetSubjectById), new { id = createdSubject.Id }, createdSubject);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSubject(int id, [FromBody] SubjectCreateDto subjectDto)
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




        // DELETE: api/Subject/{id}
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
