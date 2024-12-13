//using Application.Dtos;
//using AutoMapper;
//using DynamicExamSystem.Domain.Dtos;
//using DynamicExamSystem.infrastructure.Data;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System.Data;

////[Authorize]
//[ApiController]
//[Route("api/[controller]")]
//public class StudentExamController : ControllerBase
//{
//    private readonly IStudentExamRepository _studentExamRepository;
//    private readonly UserManager<ApplicationUser> _userManager;
//    private readonly IMapper _mapper;

//    public StudentExamController(IStudentExamRepository studentExamRepository, UserManager<ApplicationUser> userManager, IMapper mapper)
//    {
//        _studentExamRepository = studentExamRepository;
//        _userManager = userManager;
//        _mapper = mapper;
//    }

//    //[Authorize(Roles = "Student,Admin")]
//    [HttpGet("history")]
//    public async Task<IActionResult> GetStudentExamHistory()
//    {
//        var user = await _userManager.GetUserAsync(User);
//        if (user == null)
//        {
//            return Ok(user);
//        }

//        var studentExams = await _studentExamRepository.GetStudentHistoryAsync(user.Id);

//        var historyDtos = _mapper.Map<List<StudentExamHistoryDto>>(studentExams);

//        return Ok(historyDtos);
//    }
    
//}
