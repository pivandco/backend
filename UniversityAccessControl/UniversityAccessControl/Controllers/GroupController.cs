using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    private readonly IMapper _mapper;

    public GroupController(AccessControlDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<GroupDto>>> GetGroups() =>
        await _mapper.ProjectTo<GroupDto>(_db.Groups).ToListAsync();

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GroupDto>> GetGroup(int id)
    {
        var group = _mapper.Map<GroupDto>(await _db.Groups.FindAsync(id));

        return group == null ? NotFound() : group;
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutGroup(int id, GroupDto groupDto)
    {
        var group = _mapper.Map<Group>(groupDto);

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
    [Authorize]
    public async Task<ActionResult<Group>> PostGroup(GroupDto groupDto)
    {
        var group = _mapper.Map<Group>(groupDto);
        _db.Groups.Add(group);
        await _db.SaveChangesAsync();

        return CreatedAtAction("GetGroup", new { id = group.Id }, group);
    }

    [HttpDelete("{id}")]
    [Authorize]
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