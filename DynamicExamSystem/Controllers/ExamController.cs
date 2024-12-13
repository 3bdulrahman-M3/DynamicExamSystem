using Application.Dtos;
using AutoMapper;
using DynamicExamSystem.Domain.Models;
using DynamicExamSystem.infrastructure.repository.Interfaces;
using DynamicExamSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DynamicExamSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamRepository _examRepository;
        private readonly IMapper _mapper;

        public ExamController(
            IExamRepository examRepository,
            IAnswerRepository answerRepository,
            IMapper mapper)
        {
            _examRepository = examRepository;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        // Create an Exam
        [HttpPost]
        public async Task<ActionResult<ExamDto>> CreateExam([FromBody] ExamDto examDto)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var exam = new Exam
            {
                Title = examDto.Title,
                SubjectId = examDto.SubjectId
            };
            await _examRepository.AddAsync(exam);
            await _examRepository.SaveChangesAsync();
            var createdExamDto = new ExamDto
            {
                Title = exam.Title,
                SubjectId = exam.SubjectId
            };
            return Ok(createdExamDto);
        }

        [Authorize]
        [HttpGet("subject/{subjectId}")]
        public async Task<ActionResult<IEnumerable<ExamDto>>> GetExamsBySubjectId(int subjectId)
        {
            var exams = await _examRepository.GetExamsBySubjectIdAsync(subjectId);

            if (exams == null || !exams.Any())
            {
                return NotFound($"No exams found for SubjectId {subjectId}.");
            }
            var examDtos = exams.Select(exam => new ExamDto
            {
                Title = exam.Title,
                SubjectId = exam.SubjectId
            }).ToList();

            return Ok(examDtos);
        }

        [Authorize]
        [HttpGet("{examId}/questions")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestionsInExam(int examId)
        {
            var exam = await _examRepository.GetExamByIdAsync(examId);

            if (exam == null)
            {
                return NotFound($"Exam with ID {examId} not found.");
            }

            var questionDtos = exam.Questions.Select(question => new 
            {
                Text = question.Text,
                ExamId = question.ExamId,
                Answer = question.Answers.Select(a=>new
                {
                    a.Text,
                    a.IsCorrect
                })
            }).ToList();

            return Ok(questionDtos);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{examId}/questions")]
        public async Task<ActionResult<QuestionDto>> AddQuestionToExam(int examId, [FromBody] QuestionDto questionDto)
        {
            var exam = await _examRepository.GetExamByIdAsync(examId);

            if (exam == null)
            {
                return NotFound($"Exam with ID {examId} not found.");
            }
            var question = new Question
            {
                Text = questionDto.Text,  
                ExamId = examId          
            };
            exam.Questions.Add(question);

            await _examRepository.SaveChangesAsync();

            var createdQuestionDto = new QuestionDto
            {
                Text = question.Text,
                ExamId = question.ExamId
            };
            return CreatedAtAction(nameof(GetQuestionsInExam), new { examId = examId }, createdQuestionDto);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{examId}/questions/{questionId}")]
        public async Task<IActionResult> DeleteQuestionFromExam(int examId, int questionId)
        {
            var exam = await _examRepository.GetExamByIdAsync(examId);
            if (exam == null)
            {
                return NotFound($"Exam with ID {examId} not found.");
            }
            var question = exam.Questions.FirstOrDefault(q => q.Id == questionId);
            if (question == null)
            {
                return NotFound($"Question with ID {questionId} not found in the specified exam.");
            }
            _examRepository.Remove(question);
            await _examRepository.SaveChangesAsync();
            return Ok("the question was deleted"); 
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{examId}/questions/{questionId}")]
        public async Task<ActionResult<QuestionDto>> UpdateQuestionInExam(int examId, int questionId, [FromBody] QuestionEditDto questionDto)
        {
            var exam = await _examRepository.GetExamByIdAsync(examId);
            if (exam == null)
            {
                return NotFound("Exam not found.");
            }

            var question = await _examRepository.GetQuestionByIdAsync(questionId);
            if (question == null || question.ExamId != examId)  
            {
                return NotFound("Question not found in this exam.");
            }

            question.Text = questionDto.Text;   

            await _examRepository.SaveChangesAsync();
            return Ok("the question edit succesfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{examId}/edit-name")]
        public async Task<ActionResult> UpdateExamName(int examId, [FromBody] string newName)
        {
            var exam = await _examRepository.GetExamByIdAsync(examId);
            if (exam == null)
            {
                return NotFound("Exam not found.");
            }
            exam.Title = newName;

            await _examRepository.SaveChangesAsync();
            return Ok("Exam name updated successfully.");
        }



    }


}


