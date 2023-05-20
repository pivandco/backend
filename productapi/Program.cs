using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using task3_attempt2;
using task3_attempt2.Models;
using task3_attempt2.ViewModels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApiDB>(opt => opt.UseSqlite("Data Source = products.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo
    {
        Description = "REST API для работы с товарами и их категориями",
        Title = "Products API",
        Version = "v1"
    });
});

builder.Services.AddScoped<IValidator<ProductItemIn>, ProductValidator>();
builder.Services.AddScoped<IValidator<CategoryIn>, CategoryValidator>();

var app = builder.Build();
app.UseStaticFiles();


app.UseSwagger();
app.UseSwaggerUI(c => c.InjectStylesheet("/assets/css/swagger.css"));
app.UseHttpsRedirection();

var products = app.MapGroup("/products").WithTags("Работа с товарами").WithOpenApi().WithMetadata();

products.MapGet("/", GetAllProducts).Produces(200, typeof(PagedResults<ProductItemOut>))
    .WithSummary("Получение всех товаров (c пагинацией)")
    .WithDescription("ID категории можно оставлять пустым, для отображения всех товаров");
products.MapGet("/{id:int}", GetProduct).Produces(200, typeof(ProductItemOut)).ProducesProblem(404)
    .WithSummary("Получение товара по ID");
products.MapGet("/search/{query}", SearchProducts).Produces(200, typeof(ProductItemOut)).ProducesProblem(404)
    .WithSummary("Поиск товаров");
products.MapPost("/", CreateProduct).Accepts<ProductItemOut>("application/json").ProducesValidationProblem()
    .Produces(201).WithSummary("Создание товара");
products.MapPut("/{id:int}", UpdateProduct).Accepts<ProductItemOut>("application/json").ProducesValidationProblem()
    .ProducesProblem(404).Produces(204).WithSummary("Изменение товара");
products.MapDelete("/{id:int}", DeleteProduct).Produces(200).ProducesProblem(404).WithSummary("Удаление товара");

var categories = app.MapGroup("/categories").WithTags("Работа с категориями").WithOpenApi().WithMetadata();

categories.MapGet("/", GetAllCategories).Produces(200, typeof(List<CategoryOut>)).WithSummary("Получение категорий");
categories.MapGet("/{id:int}", GetCategory).Produces(200, typeof(CategoryOut)).ProducesProblem(404)
    .WithSummary("Получение названия категории по ID");
categories.MapPost("/", CreateCategory).Accepts<CategoryOut>("application/json").ProducesValidationProblem()
    .Produces(201).WithSummary("Создание категории");
categories.MapPut("/{id:int}", UpdateCategory).Accepts<CategoryOut>("application/json").ProducesValidationProblem()
    .ProducesProblem(404).Produces(204).WithSummary("Изменение названия категории");
categories.MapDelete("{id:int}", DeleteCategory).Produces(200).ProducesProblem(404).WithSummary("Удаление категории")
    .ProducesValidationProblem();

app.Run();

static async Task<IResult> GetAllProducts(ApiDB db, [FromQuery(Name = "page")] int? page = 1,
    [FromQuery(Name = "pageSize")] int? pageSize = 10, [FromQuery(Name = "categoryId")] int? categoryId = null)
{
    pageSize ??= 10;
    page ??= 1;

    var skipAmount = pageSize * (page - 1);
    var queryable = db.Products.Where(x => categoryId == null || x.CategoryId == categoryId)
        .Include(x => x.Category).AsQueryable();
    var results = await queryable.Skip(skipAmount ?? 1).Take(pageSize ?? 10).Select(x => new ProductItemOut(x))
        .ToListAsync();
    var totalNumberOfRecords = await queryable.CountAsync();
    var mod = totalNumberOfRecords % pageSize;
    var totalPageCount = totalNumberOfRecords / pageSize + (mod == 0 ? 0 : 1);

    //return TypedResults.Ok(await db.Products.Where(x => categoryId == null || x.CategoryId == categoryId)
    //    .Include(x => x.Category).Select(x => new ProductItemDTO(x)).ToListAsync());

    return TypedResults.Ok(new PagedResults<ProductItemOut>
    {
        PageNumber = page.Value,
        PageSize = pageSize!.Value,
        Results = results,
        TotalNumberOfPages = totalPageCount!.Value,
        TotalNumberOfRecords = totalNumberOfRecords
    });
}

static async Task<IResult> GetProduct(int id, ApiDB db)
{
    return await db.Products.FindAsync(id)
        is Product product
        ? TypedResults.Ok(new ProductItemOut(product))
        : TypedResults.NotFound();
}

static async Task<IResult> SearchProducts(string query, ApiDB db)
{
    var selectedProducts = await db.Products.Where(x => x.Name.ToLower().Contains(query.ToLower()))
        .Select(x => new ProductItemOut(x)).ToListAsync();
    return selectedProducts.Count > 0 ? TypedResults.Ok(selectedProducts) : TypedResults.NotFound();
}

static async Task<IResult> CreateProduct(ProductItemIn productItemDto, ApiDB db,
    IValidator<ProductItemIn> productValidator)
{
    var validationResult = productValidator.Validate(productItemDto);
    var categoryCheck = true;
    var dict = validationResult.ToDictionary();

    if (!(await db.Categories.FindAsync(productItemDto.CategoryId) is Category))
    {
        dict.Add("CategoryID", new[] { "Данной категории не существует в базе данных." });
        categoryCheck = false;
    }

    if (!validationResult.IsValid || !categoryCheck) return TypedResults.ValidationProblem(dict);
    var productItem = new Product
    {
        Name = productItemDto.Name,
        Price = productItemDto.Price,
        CategoryId = productItemDto.CategoryId
    };

    db.Products.Add(productItem);
    await db.SaveChangesAsync();

    var productItemOut = new ProductItemOut(productItem);

    return TypedResults.Created($"/products/{productItem.Id}", productItemOut);
}

static async Task<IResult> UpdateProduct(int id, ProductItemIn productItemDto, ApiDB db,
    IValidator<ProductItemIn> productValidator)
{
    var validationResult = productValidator.Validate(productItemDto);
    var categoryCheck = true;
    var dict = validationResult.ToDictionary();

    if (!(await db.Categories.FindAsync(productItemDto.CategoryId) is Category))
    {
        dict.Add("CategoryID", new[] { "Данной категории не существует в базе данных." });
        categoryCheck = false;
    }

    if (!validationResult.IsValid || !categoryCheck) return TypedResults.ValidationProblem(dict);
    var product = await db.Products.FindAsync(id);

    if (product is null) return TypedResults.NotFound();

    product.Name = productItemDto.Name;
    product.Price = productItemDto.Price;
    product.CategoryId = productItemDto.CategoryId;

    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> DeleteProduct(int id, ApiDB db)
{
    if (await db.Products.FindAsync(id) is not Product product) return TypedResults.NotFound();
    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return TypedResults.Ok();
}

static async Task<IResult> GetAllCategories(ApiDB db)
{
    return TypedResults.Ok(await db.Categories.Select(x => new CategoryOut(x)).ToListAsync());
}

static async Task<IResult> GetCategory(int id, ApiDB db)
{
    return await db.Categories.FindAsync(id)
        is Category category
        ? TypedResults.Ok(new CategoryOut(category))
        : TypedResults.NotFound();
}

static async Task<IResult> CreateCategory(ApiDB db, CategoryIn categoryDto, IValidator<CategoryIn> categoryValidator)
{
    var validationResult = categoryValidator.Validate(categoryDto);
    if (!validationResult.IsValid) return TypedResults.ValidationProblem(validationResult.ToDictionary());

    var categoryItem = new Category
    {
        Name = categoryDto.Name
    };

    db.Categories.Add(categoryItem);
    await db.SaveChangesAsync();

    var categoryOut = new CategoryOut(categoryItem);

    return TypedResults.Created($"/categories/{categoryItem.Id}", categoryOut);
}

static async Task<IResult> UpdateCategory(int id, ApiDB db, CategoryIn categoryDto,
    IValidator<CategoryIn> categoryValidator)
{
    var validationResult = categoryValidator.Validate(categoryDto);
    if (!validationResult.IsValid) return TypedResults.ValidationProblem(validationResult.ToDictionary());

    var category = await db.Categories.FindAsync(id);

    if (category is null) return TypedResults.NotFound();

    category.Name = categoryDto.Name;

    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> DeleteCategory(int id, ApiDB db)
{
    if (await db.Categories.FindAsync(id) is not Category category) return TypedResults.NotFound();
    if (await db.Products.Where(x => x.CategoryId == id).AnyAsync())
    {
        IDictionary<string, string[]> dict = new Dictionary<string, string[]>();
        dict.Add("ID",
            new[] { "Под этим ID привязаны товары. Удалите товары с данной категорией перед удалением её самой." });
        return TypedResults.ValidationProblem(dict);
    }

    db.Categories.Remove(category);
    await db.SaveChangesAsync();
    return TypedResults.Ok();
}