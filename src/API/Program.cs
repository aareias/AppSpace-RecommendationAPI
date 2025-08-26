using Shared;
using TheMovieDbSource;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var tmdbBaseUrl = 
        $"{builder.Configuration[TMDBApplicationConstants.TMDBBaseUrl]}/{builder.Configuration[TMDBApplicationConstants.TMDBApiVersion]}";

builder.Services.AddHttpClient<TMDBClient>(o =>
{
    o.BaseAddress = new Uri(tmdbBaseUrl);
});

ApplicationSettings.SetConfiguration(builder.Configuration);

builder.Services.RegisterTMDBServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();