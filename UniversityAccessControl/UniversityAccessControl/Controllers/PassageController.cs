using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAccessControl.Models;

namespace UniversityAccessControl.Dto;

[ApiController]
[Route("[controller]")]
public class PassageController : ControllerBase
{
    private readonly AccessControlDbContext _db;

    public PassageController(AccessControlDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Passage>>> GetPassages() => await _db.Passages.ToListAsync();

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Passage>> GetPassage(int id)
    {
        var passage = await _db.Passages.FindAsync(id);

        return passage == null ? NotFound() : passage;
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutPassage(int id, Passage passage)
    {
        if (id != passage.Id)
        {
            return BadRequest();
        }

        _db.Entry(passage).State = EntityState.Modified;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PassageExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Passage>> PostPassage(Passage passage)
    {
        _db.Passages.Add(passage);
        await _db.SaveChangesAsync();

        return CreatedAtAction("GetPassage", new { id = passage.Id }, passage);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletePassage(int id)
    {
        var passage = await _db.Passages.FindAsync(id);
        if (passage == null)
        {
            return NotFound();
        }

        _db.Passages.Remove(passage);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private bool PassageExists(int id)
    {
        return _db.Passages.Any(e => e.Id == id);
    }
}