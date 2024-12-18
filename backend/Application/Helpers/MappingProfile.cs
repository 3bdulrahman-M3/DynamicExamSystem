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
            CreateMap<QuestionEditDto, Question>();

            CreateMap<Question, QuestionsDto>()
               .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));

            // Map Answer to AnswerDto
            CreateMap<Answer, AnswerDto>();
            CreateMap<Answer, OptionDto>();
            CreateMap<Answer, OptionsDto>();

            CreateMap<OptionDto, Answer>()
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect));

            CreateMap<ApplicationUser, ApplicationUserDto>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        }
    }
}
