using AutoMapper;
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
    private readonly IMapper _mapper;

    public RuleController(AccessControlDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RuleResponse>>> GetRules() =>
        await _mapper.ProjectTo<RuleResponse>(_db.Rules).ToListAsync();

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<RuleResponse>> GetRule(int id)
    {
        var rule = _mapper.Map<RuleResponse>(await _db.Rules.FindAsync(id));

        return rule == null ? NotFound() : rule;
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutRule(int id, RuleRequest ruleDto)
    {
        var rule = _mapper.Map<Rule>(ruleDto);

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
    public async Task<ActionResult<RuleResponse>> PostRule(RuleRequest ruleDto)
    {
        var rule = _mapper.Map<Rule>(ruleDto);
        _db.Rules.Add(rule);
        await _db.SaveChangesAsync();

        return CreatedAtAction("GetRule", new { id = rule.Id }, _mapper.Map<RuleResponse>(rule));
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