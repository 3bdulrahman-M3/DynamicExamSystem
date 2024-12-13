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

        }
    }
}
