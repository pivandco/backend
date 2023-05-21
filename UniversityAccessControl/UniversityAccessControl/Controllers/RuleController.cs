using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAccessControl.Dto;
using UniversityAccessControl.Models;

namespace UniversityAccessControl.Controllers;

[ApiController]
[Route("[controller]")]
public class RuleController : ControllerBase
{
    private readonly AccessControlDbContext _db;

    public RuleController(AccessControlDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Rule>>> GetRules() => await _db.Rules.ToListAsync();

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Rule>> GetRule(int id)
    {
        var rule = await _db.Rules.FindAsync(id);

        return rule == null ? NotFound() : rule;
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutRule(int id, RuleRequest ruleDto)
    {
        var rule = await ruleDto.ToModelAsync(_db.Subjects, _db.Groups, _db.Passages, _db.Areas);
        
        if (id != rule.Id)
        {
            return BadRequest();
        }

        _db.Entry(rule).State = EntityState.Modified;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RuleExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Rule>> PostRule(RuleRequest ruleDto)
    {
        var rule = await ruleDto.ToModelAsync(_db.Subjects, _db.Groups, _db.Passages, _db.Areas);
        _db.Rules.Add(rule);
        await _db.SaveChangesAsync();

        return CreatedAtAction("GetRule", new { id = rule.Id }, rule);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteRule(int id)
    {
        var rule = await _db.Rules.FindAsync(id);
        if (rule == null)
        {
            return NotFound();
        }

        _db.Rules.Remove(rule);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private bool RuleExists(int id)
    {
        return _db.Rules.Any(e => e.Id == id);
    }
}