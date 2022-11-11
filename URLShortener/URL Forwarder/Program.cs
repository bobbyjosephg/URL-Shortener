//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

using StackExchange.Redis;
using UrlShortener.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ShortenedUrlsDb")
            ?? throw new Exception("Missing 'ShortenedUrlsDb' connection string");
var redisConnection = await ConnectionMultiplexer.ConnectAsync(connectionString);
builder.Services.AddSingleton(redisConnection);
builder.Services.AddTransient<ShortUrlRepository>();

var app = builder.Build();

app.MapGet("/{path}", async (
    string path,
    ShortUrlRepository shortUrlRepository
) =>
{
    if (ShortUrlValidator.ValidatePath(path, out _))
        return Results.BadRequest();

    var shortUrl = await shortUrlRepository.Get(path);
    if (shortUrl == null || string.IsNullOrEmpty(shortUrl.Destination))
        return Results.NotFound();

    return Results.Redirect(shortUrl.Destination);
});

app.Run();
