using Microsoft.EntityFrameworkCore;
using ProjetAsp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MonDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MaConnexion")));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

