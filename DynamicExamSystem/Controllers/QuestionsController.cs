using Application.Dtos;
using AutoMapper;
using DynamicExamSystem.Domain.Models;
using DynamicExamSystem.infrastructure.Data;
using DynamicExamSystem.infrastructure.repository.Implementations;
using DynamicExamSystem.infrastructure.repository.Interfaces;
using DynamicExamSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicExamSystem.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        public QuestionsController(IQuestionRepository questionRepository, IMapper mapper, AppDbContext context, IAnswerRepository answerRepository)
        {
            _context = context;
            _questionRepository = questionRepository;
            _mapper = mapper;
            _answerRepository = answerRepository;
        }

        [HttpPost("Answer")]
        public async Task<ActionResult> AddAnswer(int questionId, [FromBody] OptionDto answerDto)
        {
            // Step 1: Get the question by its ID
            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null)
            {
                return NotFound("Question not found.");
            }

            // Step 2: Map the DTO to the Answer entity
            var answer = new Answer
            {
                Text = answerDto.Text,
                IsCorrect = answerDto.IsCorrect,
                QuestionId = questionId
            };

            await _answerRepository.AddAsync(answer);
            await _answerRepository.SaveChangesAsync();

            return Ok("Answer added successfully.");
        }

        [HttpDelete("{answerId}")]
        public async Task<ActionResult> DeleteAnswer(int questionId, int answerId)
        {
            var answer = await _answerRepository.GetByIdAsync(answerId);
            if (answer == null || answer.QuestionId != questionId)
            {
                return NotFound("Answer not found or does not belong to the specified question.");
            }

            _answerRepository.Delete(answer);
            await _answerRepository.SaveChangesAsync();

            return Ok("Answer deleted successfully.");
        }

        [HttpPut("{answerId}")]
        public async Task<ActionResult> UpdateAnswer(int answerId, [FromBody] OptionDto answerDto)
        {
            
            var answer = await _answerRepository.GetByIdAsync(answerId);
            if (answer == null)
            {
                return NotFound("Answer not found.");
            }

            answer.Text = answerDto.Text;
            answer.IsCorrect = answerDto.IsCorrect;

            _answerRepository.Update(answer);
            await _answerRepository.SaveChangesAsync();

            return Ok("Answer updated successfully.");
        }


    }
}
