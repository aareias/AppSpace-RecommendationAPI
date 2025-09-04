using API.IoC;
using SessionsDB.Configuration;
using Utils;
using TMDB.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient();

ApplicationSettings.SetConfiguration(builder.Configuration);

builder.Services.RegisterServices();

// Register SessionsDB repositories
builder.Services.AddSessionsDBRepositories();

// Register TMDB services
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