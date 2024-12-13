using Application.Dtos;
using AutoMapper;
using DynamicExamSystem.Domain.Models;
using DynamicExamSystem.infrastructure.Data;
using DynamicExamSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class SubjectRepository : ISubjectRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public SubjectRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync()
    {
        var subjects = await _context.Subjects.ToListAsync();
        return _mapper.Map<IEnumerable<SubjectDto>>(subjects);
    }

    public async Task<SubjectDto?> GetSubjectByIdAsync(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null) return null;
        return _mapper.Map<SubjectDto>(subject);
    }

    public async Task<SubjectDto> CreateSubjectAsync(Subject subject)
    {
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
        return _mapper.Map<SubjectDto>(subject);
    }

    public async Task<Subject> GetByIdAsync(int id)
    {
        return await _context.Subjects.FindAsync(id);
    }

    public void Update(Subject subject)
    {
        _context.Subjects.Update(subject);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }


    public async Task<bool> DeleteSubjectAsync(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null) return false;

        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync();
        return true;
    }
}

