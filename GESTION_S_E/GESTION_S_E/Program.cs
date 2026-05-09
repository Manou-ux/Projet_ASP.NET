using Microsoft.EntityFrameworkCore;
using ProjetAsp.Models;

var builder = WebApplication.CreateBuilder(args);

// MVC + Views
builder.Services.AddControllersWithViews();

// DbContext PostgreSQL
builder.Services.AddDbContext<MonDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MaConnexion")));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

// Route MVC par défaut
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();