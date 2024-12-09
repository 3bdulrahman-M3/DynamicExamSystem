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
using static Application.Dtos.CreateAnswerDto;

namespace Application.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Exam, ExamDto>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name));
            CreateMap<CreateExamDto, Exam>();

            CreateMap<Question, QuestionDto>()
            .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Answers));
            CreateMap<CreateQuestionDto, Question>();

            CreateMap<Answer, OptionsDto>().ReverseMap();
            CreateMap<CreateAnswerDto.OptionDto, Answer>().ReverseMap();

            CreateMap<StudentExamHestory, StudentExamHistoryDto>()
             .ForMember(dest => dest.ExamName, opt => opt.MapFrom(src => src.Exam.Title));


            CreateMap<Answer, OptionDto>();

            CreateMap<ApplicationUser, ApplicationUserDto>();
        }
    }
}
