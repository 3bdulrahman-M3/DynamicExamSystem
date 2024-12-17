using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DynamicExamSystem.infrastructure.Data;
using DynamicExamSystem.Domain.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace DynamicExamSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        [Authorize(Roles = "Student, Admin")]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logout successful." });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllStudents()
        {
            var allUsers = await _userManager.GetUsersInRoleAsync("Student");
            if (allUsers == null || !allUsers.Any())
            {
                return NotFound("No students found.");
            }
            var studentDtos = _mapper.Map<List<ApplicationUserDto>>(allUsers);

            return Ok(studentDtos);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest("Failed to delete user.");
        }

        [Authorize(Roles = " Admin,Student")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var userDto = new
            {
                user.UserName,
                user.Email,
            };

            return Ok(userDto);
        }


    }
}
