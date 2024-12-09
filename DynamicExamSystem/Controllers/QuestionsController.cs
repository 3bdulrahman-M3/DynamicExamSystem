using Application.Dtos;
using AutoMapper;
using DynamicExamSystem.infrastructure.Data;
using DynamicExamSystem.infrastructure.repository.Interfaces;
using DynamicExamSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicExamSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        public QuestionsController(IQuestionRepository questionRepository, IMapper mapper,AppDbContext context)
        {
            _context = context;
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        [Authorize(Roles = "Student, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestions()
        {
            var questions = await _questionRepository.GetAllQuestionsWithAnswersAsync();
            var questionDtos = _mapper.Map<List<QuestionDto>>(questions);

            return Ok(questionDtos);
        }

        [Authorize(Roles = "Student, Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestion(int id)
        {
            var question = await _questionRepository.GetByIdWithAnswersAsync(id);
            if (question == null)
            {
                return NotFound($"Question with ID {id} not found.");
            }
            var questionDto = _mapper.Map<QuestionDto>(question);
            return Ok(questionDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data.");
            }
            var question = _mapper.Map<Question>(dto);
            var createdQuestion = await _questionRepository.AddAsync(question);

            var questionDto = _mapper.Map<QuestionDto>(createdQuestion);

            return Ok(dto.Text);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, [FromForm] CreateQuestionDto dto)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound($"Question with ID {id} not found.");
            }

            _mapper.Map(dto, question);
            _questionRepository.Update(question);
            await _questionRepository.SaveChangesAsync();

            return Ok(question);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound($"Question with ID {id} not found.");
            }

            _questionRepository.Remove(question);
            await _questionRepository.SaveChangesAsync();

            return Ok(question);
        }


    }
}
