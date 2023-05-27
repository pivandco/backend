using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAccessControl.Dto;
using UniversityAccessControl.Models;

namespace UniversityAccessControl.Controllers;

[ApiController]
[Route("[controller]")]
public class SubjectController : ControllerBase
{
    private readonly AccessControlDbContext _db;
    private readonly IMapper _mapper;

    public SubjectController(AccessControlDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubjectResponse>>> GetSubjects() =>
        await _mapper.ProjectTo<SubjectResponse>(_db.Subjects).ToListAsync();

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<SubjectResponse>> GetSubject(int id)
    {
        var subject =
            _mapper.Map<SubjectResponse>(await _db.Subjects.Include(s => s.Groups).FirstAsync(s => s.Id == id));

        return subject == null ? NotFound() : subject;
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutSubject(int id, SubjectPutRequest subjectPutDto)
    {
        if (id != subjectPutDto.Id)
        {
            return BadRequest();
        }

        var subject = await _db.Subjects.Include(s => s.Groups).FirstAsync(s => s.Id == id);

        _mapper.Map(subjectPutDto, subject);
        subject.Groups = _db.Groups.Where(g => subjectPutDto.GroupIds.Contains(g.Id)).ToList();

        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<SubjectResponse>> PostSubject(SubjectPostRequest subjectPutDto)
    {
        var subject = _mapper.Map<Subject>(subjectPutDto);
        var groupIds = subject.Groups.Select(g => g.Id).ToArray();
        subject.Groups = _db.Groups.Where(g => groupIds.Contains(g.Id)).ToList();
        _db.Subjects.Add(subject);
        await _db.SaveChangesAsync();

        return CreatedAtAction("GetSubject", new { id = subject.Id }, _mapper.Map<SubjectResponse>(subject));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        var subject = await _db.Subjects.FindAsync(id);
        if (subject == null)
        {
            return NotFound();
        }

        _db.Subjects.Remove(subject);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private bool SubjectExists(int id)
    {
        return _db.Subjects.Any(e => e.Id == id);
    }
}