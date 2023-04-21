using System.Drawing;
using System.Drawing.Imaging;
using System.Text.Json;
using greetings_card;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async context => { await Results.Ok().ExecuteAsync(context); });

app.MapGet("/templates",
    async context => { await Results.Text(File.ReadAllText("templates.json")).ExecuteAsync(context); });

app.MapGet("/img", ImageGet);

app.Run();

async Task ImageGet(HttpContext context)
{
    var templates = ReadTemplates();

    string? firstname = context.Request.Query["firstname"];
    string? lastname = context.Request.Query["lastname"];
    string? holiday = context.Request.Query["holiday"];
    string? download = context.Request.Query["download"];

    if (firstname != null && lastname != null && holiday != null && templates.ContainsKey(holiday))
    {
#pragma warning disable CA1416
        var currentHoliday = templates[holiday];

        var bitmap = Image.FromFile("img/" + currentHoliday.pathToIMG);

        var graphicsImage = Graphics.FromImage(bitmap);

        var stringFormat = new StringFormat();
        stringFormat.Alignment = currentHoliday.alignment;


        var stringColor = ColorTranslator.FromHtml(currentHoliday.colorForeground);
        var textOnImage = currentHoliday.message.Replace("{firstname}", firstname).Replace("{lastname}", lastname);
        graphicsImage.DrawString(textOnImage,
            new Font(currentHoliday.fontFamily, currentHoliday.fontSize, FontStyle.Regular),
            new SolidBrush(stringColor), new Point(currentHoliday.point[0], currentHoliday.point[1]), stringFormat);


        var memorySteam = new MemoryStream();

        bitmap.Save(memorySteam, ImageFormat.Png);
        var result = memorySteam.ToArray();

#pragma warning restore CA1416
        context.Response.ContentType = "image/png";
        if (download == "1") context.Response.Headers.Add("content-disposition", $"attachment; filename={holiday}.png");

        await context.Response.Body.WriteAsync(result);
    }
    else
    {
        await Results.BadRequest("Invalid input").ExecuteAsync(context);
    }
}

Dictionary<string, Template> ReadTemplates()
{
    var options = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };
    var jsonFile = "templates.json";
    var jsonFileString = File.ReadAllText(jsonFile);
    return JsonSerializer.Deserialize<Dictionary<string, Template>>(jsonFileString, options);
    
}