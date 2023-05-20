using Microsoft.AspNetCore.Authorization;

namespace RestaurantAutomation.Controllers;

using Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

[ApiController]
[Route("[controller]")]
public sealed class DishController : ControllerBase
{
    private readonly RestaurantAutomationDbContext _db;

    public DishController(RestaurantAutomationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IEnumerable<DishResponseDto>> Index() =>
        await _db.Dishes.Include(d => d.DishTags).Select(d => DishResponseDto.FromDish(d)).ToListAsync();

    /// <response code="404">If a dish with the specified ID does not exist</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DishResponseDto>> Get(int id) =>
        await _db.Dishes.FindAsync(id) is { } dish ? Ok(DishResponseDto.FromDish(dish)) : NotFound();

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<DishResponseDto>> Create(DishRequestDto dishInput)
    {
        var dish = dishInput.ToDish(_db.DishTags);
        _db.Dishes.Add(dish);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = dish.Id }, DishResponseDto.FromDish(dish));
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, DishRequestDto input)
    {
        if (id != input.Id)
        {
            return Problem("URL ID and body ID do not match", statusCode: 400, title: "ID mismatch");
        }

        var dish = input.ToDish(_db.DishTags);
        _db.Entry(dish).State = EntityState.Modified;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DishExists(dish))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var dish = await _db.Dishes.FindAsync(id);
        if (dish == null) return NotFound();

        _db.Dishes.Remove(dish);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private bool DishExists(Dish dish) => _db.Dishes.Any(d => d.Id == dish.Id);
}
