using Microsoft.AspNetCore.Authorization;

namespace RestaurantAutomation.Controllers;

using Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("[controller]")]
[ApiController]
public class DishTagController : ControllerBase
{
    private readonly RestaurantAutomationDbContext _db;

    public DishTagController(RestaurantAutomationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IEnumerable<DishTagDto>> List() =>
        await _db.DishTags.Select(t => DishTagDto.FromDishTag(t)).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DishTagDto>> Get(int id) =>
        await _db.DishTags.FindAsync(id) is { } tag ? DishTagDto.FromDishTag(tag) : NotFound();

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, DishTagDto dishTagDto)
    {
        if (id != dishTagDto.Id)
        {
            return BadRequest();
        }

        var dishTag = dishTagDto.ToDishTag();
        _db.Entry(dishTag).State = EntityState.Modified;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DishTagExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    private bool DishTagExists(int id)
    {
        return _db.DishTags.Any(e => e.Id == id);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<DishTagDto>> Create(DishTagDto dishTagDto)
    {
        var tag = dishTagDto.ToDishTag();
        _db.DishTags.Add(tag);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = tag.Id }, tag);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var dishTag = await _db.DishTags.FindAsync(id);
        if (dishTag == null)
        {
            return NotFound();
        }

        _db.DishTags.Remove(dishTag);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
