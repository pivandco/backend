using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAccessControl.Dto;
using UniversityAccessControl.Models;

namespace UniversityAccessControl.Controllers;

[ApiController]
[Route("[controller]")]
public class AreaController : ControllerBase
{
    private readonly AccessControlDbContext _db;

    public AreaController(AccessControlDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Area>>> GetAreas()
    {
        return await _db.Areas.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Area>> GetArea(int id)
    {
        var area = await _db.Areas.FindAsync(id);

        return area == null ? NotFound() : area;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutArea(int id, AreaRequest areaDto)
    {
        var area = areaDto.ToModel();

        if (id != area.Id) return BadRequest();

        _db.Entry(area).State = EntityState.Modified;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AreaExists(id)) return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Area>> PostArea(AreaRequest area)
    {
        _db.Areas.Add(area.ToModel());
        await _db.SaveChangesAsync();

        return CreatedAtAction("GetArea", new { id = area.Id }, area);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArea(int id)
    {
        var area = await _db.Areas.FindAsync(id);
        if (area == null) return NotFound();

        _db.Areas.Remove(area);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private bool AreaExists(int id)
    {
        return _db.Areas.Any(e => e.Id == id);
    }
}