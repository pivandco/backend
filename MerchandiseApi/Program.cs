using FluentValidation;
using MerchandiseApi;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MerchandiseApiDb>(opt => opt.UseSqlite("Data Source=app.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<SieveProcessor>();

builder.Services.AddScoped<IValidator<ProductDto>, ProductValidator>();
builder.Services.AddScoped<IValidator<ProductCategoryDto>, ProductCategoryValidator>();

var app = builder.Build();

var productsApi = app.MapGroup("/api/products");

productsApi.MapGet("/",
    async (MerchandiseApiDb db, SieveProcessor sieveProcessor, int? page, int? pageSize, int? categoryId) =>
    {
        var sieveModel = new SieveModel { Page = page, PageSize = pageSize };
        var products = db.Products
            .Where(p => categoryId == null || p.ProductCategoryId == categoryId)
            .Include(p => p.ProductCategory)
            .Select(p => new ProductDto(p))
            .AsNoTracking();

        return await sieveProcessor.Apply(sieveModel, products).ToListAsync();
    });

productsApi.MapGet("/{id:int}", async (int id, MerchandiseApiDb db) =>
    await db.FindAsync<Product>(id) is { } product ? Results.Ok(new ProductDto(product)) : Results.NotFound());

productsApi.MapPost("/", async (MerchandiseApiDb db, ProductDto productDto, IValidator<ProductDto> productValidator) =>
{
    var validationResult = productValidator.Validate(productDto);
    if (!validationResult.IsValid)
        return Results.ValidationProblem(validationResult.ToDictionary());

    db.Products.Add(productDto.ToProduct());
    await db.SaveChangesAsync();
    return Results.Created($"/api/products/{productDto.Id}", productDto);
});

productsApi.MapPut("/{id:int}",
    async (int id, ProductDto newProductDto, MerchandiseApiDb db, IValidator<ProductDto> productValidator) =>
    {
        var validationResult = productValidator.Validate(newProductDto);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var todo = await db.Products.FindAsync(id);
        if (todo == null) return Results.NotFound();

        todo.Name = newProductDto.Name;
        todo.Price = newProductDto.Price;
        todo.ProductCategoryId = newProductDto.CategoryId;

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

var categoriesApi = app.MapGroup("/api/categories");

categoriesApi.MapGet("/",
    async (MerchandiseApiDb db) => await db.ProductCategories
        .Select(c => new ProductCategoryDto(c))
        .ToListAsync());

categoriesApi.MapGet("/{id:int}",
    async (MerchandiseApiDb db, int id) => await db.ProductCategories.FindAsync(id) is { } category
        ? Results.Ok(new ProductCategoryDto(category))
        : Results.NotFound());

categoriesApi.MapPost("/",
    async (MerchandiseApiDb db, ProductCategoryDto categoryDto, IValidator<ProductCategoryDto> categoryValidator) =>
    {
        var validationResult = categoryValidator.Validate(categoryDto);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var category = categoryDto.ToProductCategory();
        db.ProductCategories.Add(category);
        await db.SaveChangesAsync();
        return Results.Created($"/api/categories/{category.Id}", new ProductCategoryDto(category));
    });

categoriesApi.MapPut("/{id:int}",
    async (int id, MerchandiseApiDb db, ProductCategoryDto newCategoryDto,
        IValidator<ProductCategoryDto> categoryValidator) =>
    {
        var validationResult = categoryValidator.Validate(newCategoryDto);
        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());
        
        var oldCategory = await db.ProductCategories.FindAsync(id);
        if (oldCategory == null) return Results.NotFound();
        oldCategory.Name = newCategoryDto.Name;
        await db.SaveChangesAsync();
        return Results.NoContent();
    });

categoriesApi.MapDelete("/{id:int}", async (int id, MerchandiseApiDb db) =>
{
    var category = await db.ProductCategories.FindAsync(id);
    if (category == null) return Results.NotFound();
    db.ProductCategories.Remove(category);
    await db.SaveChangesAsync();
    return Results.Ok(category);
});

app.Run();
