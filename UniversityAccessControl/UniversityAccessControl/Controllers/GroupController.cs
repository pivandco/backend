using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAccessControl.Dto;
using UniversityAccessControl.Models;

namespace UniversityAccessControl.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupController : ControllerBase
{
    private readonly AccessControlDbContext _db;

    public GroupController(AccessControlDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
    {
        return await _db.Groups.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> GetGroup(int id)
    {
        var group = await _db.Groups.FindAsync(id);

        return group == null ? NotFound() : group;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutGroup(int id, GroupRequest groupDto)
    {
        var group = groupDto.ToModel();

        if (id != group.Id) return BadRequest();

        _db.Entry(group).State = EntityState.Modified;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GroupExists(id)) return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Group>> PostGroup(GroupRequest group)
    {
        _db.Groups.Add(group.ToModel());
        await _db.SaveChangesAsync();

        return CreatedAtAction("GetGroup", new { id = group.Id }, group);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        var group = await _db.Groups.FindAsync(id);
        if (group == null) return NotFound();

        _db.Groups.Remove(group);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private bool GroupExists(int id)
    {
        return _db.Groups.Any(e => e.Id == id);
    }
}