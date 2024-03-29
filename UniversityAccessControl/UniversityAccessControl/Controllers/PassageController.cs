using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAccessControl.Dto;
using UniversityAccessControl.Models;

namespace UniversityAccessControl.Controllers;

[ApiController]
[Route("[controller]")]
public class PassageController : ControllerBase
{
    private readonly AccessControlDbContext _db;
    private readonly IMapper _mapper;

    public PassageController(AccessControlDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<PassageResponse>>> GetPassages() =>
        await _mapper.ProjectTo<PassageResponse>(_db.Passages.Include(p => p.Area)).ToListAsync();

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<PassageResponse>> GetPassage(int id)
    {
        var passage = _mapper.Map<PassageResponse>(await _db.Passages.Include(p => p.Area).FirstAsync(p => p.Id == id));

        return passage == null ? NotFound() : passage;
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutPassage(int id, PassagePutRequest passagePutDto)
    {
        var passage = _mapper.Map<Passage>(passagePutDto);

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
    public async Task<ActionResult<PassageResponse>> PostPassage(PassagePostRequest passagePutDto)
    {
        var passage = _mapper.Map<Passage>(passagePutDto);
        _db.Passages.Add(passage);
        await _db.Entry(passage).Reference(p => p.Area).LoadAsync();
        await _db.SaveChangesAsync();

        return CreatedAtAction("GetPassage", new { id = passage.Id }, _mapper.Map<PassageResponse>(passage));
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