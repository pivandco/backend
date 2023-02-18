using MerchandiseApi;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MerchandiseApiDb>(opt => opt.UseSqlite("Data Source=app.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<SieveProcessor>();
var app = builder.Build();

var productsApi = app.MapGroup("/api/products");

productsApi.MapGet("/",
    async (MerchandiseApiDb db, SieveProcessor sieveProcessor, int? page, int? pageSize) =>
        await sieveProcessor.Apply(new SieveModel { Page = page, PageSize = pageSize }, db.Products.AsNoTracking())
            .ToListAsync());

productsApi.MapGet("/{id:int}", async (int id, MerchandiseApiDb db) =>
    await db.FindAsync<Product>(id) is { } product ? Results.Ok(product) : Results.NotFound());

productsApi.MapPost("/", async (MerchandiseApiDb db, Product product) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/api/products/{product.Id}", product);
});

productsApi.MapPut("/{id:int}", async (int id, Product newProduct, MerchandiseApiDb db) =>
{
    var todo = await db.Products.FindAsync(id);
    if (todo == null) return Results.NotFound();

    todo.Name = newProduct.Name;
    todo.Price = newProduct.Price;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

productsApi.MapDelete("/{id:int}", async (int id, MerchandiseApiDb db) =>
{
    if (await db.Products.FindAsync(id) is not { } todo) return Results.NotFound();

    db.Products.Remove(todo);
    await db.SaveChangesAsync();
    return Results.Ok(todo);
});

app.Run();