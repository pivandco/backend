using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAccessControl.Dto;
using UniversityAccessControl.Models;

namespace UniversityAccessControl.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectController : ControllerBase
{
    private readonly AccessControlDbContext _db;

    public SubjectController(AccessControlDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects() => await _db.Subjects.ToListAsync();

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Subject>> GetSubject(int id)
    {
        var subject = await _db.Subjects.FindAsync(id);

        return subject == null ? NotFound() : subject;
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutSubject(int id, SubjectRequest subjectDto)
    {
        var subject = await subjectDto.ToModelAsync(_db.Groups);

        if (id != subject.Id)
        {
            return BadRequest();
        }

        _db.Entry(subject).State = EntityState.Modified;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SubjectExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Subject>> PostSubject(SubjectRequest subjectDto)
    {
        var subject = await subjectDto.ToModelAsync(_db.Groups);
        _db.Subjects.Add(subject);
        await _db.SaveChangesAsync();

        return CreatedAtAction("GetSubject", new { id = subject.Id }, subject);
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