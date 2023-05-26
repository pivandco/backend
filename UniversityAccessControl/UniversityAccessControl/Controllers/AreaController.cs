using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    private readonly IMapper _mapper;

    public AreaController(AccessControlDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<AreaDto>>> GetAreas() => await _mapper.ProjectTo<AreaDto>(_db.Areas).ToListAsync();

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<AreaDto>> GetArea(int id)
    {
        var area = _mapper.Map<AreaDto>(await _db.Areas.FindAsync(id));

        return area == null ? NotFound() : area;
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutArea(int id, AreaDto areaDto)
    {
        var area = _mapper.Map<Area>(areaDto);

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
    [Authorize]
    public async Task<ActionResult<Area>> PostArea(AreaPostRequest areaDto)
    {
        var area = _mapper.Map<Area>(areaDto);
        _db.Areas.Add(area);
        await _db.SaveChangesAsync();

        return CreatedAtAction("GetArea", new { id = area.Id }, area);
    }

    [HttpDelete("{id}")]
    [Authorize]
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