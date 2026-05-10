using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// DbContext PostgreSQL
builder.Services.AddDbContext<MonDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("MaConnexion")
    )
);

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}"
);

app.Run();