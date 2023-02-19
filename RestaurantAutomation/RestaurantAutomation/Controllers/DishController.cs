namespace RestaurantAutomation.Controllers;

using Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        await _db.Dishes.Select(d => DishResponseDto.FromDish(d)).ToListAsync();

    /// <response code="404">If a dish with the specified ID does not exist</response>
    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DishResponseDto>> Get(int id) =>
        await _db.Dishes.FindAsync(id) is { } dish ? Ok(DishResponseDto.FromDish(dish)) : NotFound();

    [HttpPost]
    public async Task<ActionResult<DishResponseDto>> Create(DishRequestDto dishInput)
    {
        var dish = dishInput.ToDish(_db.DishTags);
        _db.Dishes.Add(dish);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = dish.Id }, DishResponseDto.FromDish(dish));
    }
}
