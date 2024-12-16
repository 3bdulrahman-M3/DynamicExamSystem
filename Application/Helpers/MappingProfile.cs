using Application.Dtos;
using AutoMapper;
using DynamicExamSystem.Domain.Dtos;
using DynamicExamSystem.Domain.Models;
using DynamicExamSystem.infrastructure.Data;
using DynamicExamSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static Application.Dtos.CreateAnswerDto;

namespace Application.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //subject mapper
            CreateMap<Subject, SubjectDto>();
            CreateMap<SubjectDto, Subject>();

            CreateMap<Exam, ExamDto>();
            CreateMap<CreateExamDto, Exam>();
            CreateMap<Exam, CreateExamDto>();

            // Map Question to QuestionDto
            CreateMap<Question, QuestionDto>();
            CreateMap<QuestionDto, Question>(); // For updating questions

            // Map QuestionEditDto to Question (for updates)
            CreateMap<QuestionEditDto, Question>();

            CreateMap<Question, QuestionsDto>()
               .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));  // Maps Answers to AnswerDto

            // Map Answer to AnswerDto
            CreateMap<Answer, AnswerDto>();
            CreateMap<Answer, OptionDto>();
            CreateMap<Answer, OptionsDto>();

            CreateMap<OptionDto, Answer>()
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect));

        }
    }
}
