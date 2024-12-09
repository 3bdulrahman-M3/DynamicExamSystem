using Application.Dtos;
using AutoMapper;
using DynamicExamSystem.Domain.Models;
using DynamicExamSystem.infrastructure.repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicExamSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IMapper _mapper;

        public AnswerController(IAnswerRepository answerRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        
        [HttpPost("AddAnswers")]
        public async Task<ActionResult> AddAnswers([FromBody] CreateAnswerDto dto)
        {
            var answers = _mapper.Map<List<Answer>>(dto.Options);
            foreach (var answer in answers)
            {
                answer.QuestionId = dto.QuestionId;
            }

            await _answerRepository.AddRangeAsync(answers);
            await _answerRepository.SaveChangesAsync();

            return Ok($"Answers have been successfully added to Question ID {dto.QuestionId}.");
        }

        [Authorize(Roles = "Student, Admin")]
        [HttpGet("AllAnswers")]
        public async Task<ActionResult> GetAllAnswers()
        {
            var answers = await _answerRepository.GetAllAsync();
            var answerDtos = _mapper.Map<List<OptionsDto>>(answers);

            return Ok(answerDtos);
        }

        [Authorize(Roles = "Student, Admin")]
        [HttpGet("AnswersByQuestion/{questionId}")]
        public async Task<ActionResult> GetAnswersByQuestion(int questionId)
        {
            var answers = await _answerRepository.FindAsync(a => a.QuestionId == questionId);

            if (!answers.Any())
            {
                return NotFound($"No answers found for Question ID {questionId}.");
            }

            var answerDtos = _mapper.Map<List<OptionsDto>>(answers);
            return Ok(answerDtos);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateAnswer/{id}")]
        public async Task<ActionResult> UpdateAnswer(int id, [FromBody] OptionsDto dto)
        {
            var answer = await _answerRepository.GetByIdAsync(id);
            if (answer == null)
            {
                return NotFound($"Answer with ID {id} not found.");
            }

            _mapper.Map(dto, answer);
            _answerRepository.Update(answer);
            await _answerRepository.SaveChangesAsync();

            return Ok(answer);
        }
        [Authorize("Admin")]
        [HttpDelete("DeleteAnswer/{id}")]
        public async Task<ActionResult> DeleteAnswer(int id)
        {
            var answer = await _answerRepository.GetByIdAsync(id);
            if (answer == null)
            {
                return NotFound($"Answer with ID {id} not found.");
            }

            _answerRepository.Remove(answer);
            await _answerRepository.SaveChangesAsync();

            return Ok($"Answer with ID {id} deleted successfully.");
        }
    }
}
