using DynamicExamSystem.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DynamicExamSystem.infrastructure.Data;
using Application.Dtos;
using System.Collections.Immutable;
using DynamicExamSystem.Models;
using AutoMapper;

namespace DynamicExamSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly AppDbContext _context;
        

        public SubjectController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("AllSubject")]
        public async Task<ActionResult> GetSubject()
        {
            var supjects= await _context.Subjects.ToListAsync();
            return Ok(supjects);
        }
        //Include(s => s.Exams)



        [HttpPost]
        public async Task<ActionResult> CreateSubject([FromForm] SubjectDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data.");
            }

            var subject = new Subject
            {
                Name = dto.Name
            };

            await _context.Subjects.AddAsync(subject); 
            await _context.SaveChangesAsync();          

            return Ok(subject); 
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSupject(int id, [FromForm] SubjectDto dto)
        {
            var subject = await _context.Subjects.FindAsync(id);  
            if (subject == null)
            {
                return NotFound("Subject not found.");
            }

            
            subject.Name = dto.Name;
            

            _context.Subjects.Update(subject);  
            await _context.SaveChangesAsync(); 

            return Ok(subject);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id); 
            if (subject == null)
            {
                return NotFound("Subject not found.");
            }

            _context.Subjects.Remove(subject);  
            await _context.SaveChangesAsync(); 

            return Ok(subject);
        }

    }
}
