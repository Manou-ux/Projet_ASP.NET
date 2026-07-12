using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using GESTION_S_E.Models;
using GESTION_S_E.Services;  // ← Ajout pour IEmailSender et EmailSettings

var builder = WebApplication.CreateBuilder(args);

// 1. Ajouter MVC
builder.Services.AddControllersWithViews();

// 2. Ajouter l'authentification (Cookies)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

// 3. Ajouter l'autorisation
builder.Services.AddAuthorization();

// 4. DbContext PostgreSQL
builder.Services.AddDbContext<MonDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MaConnexion")));

// 5. Enregistrement des services email
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

// Pour bien gérer les types PostgreSQL (interval + timestamp with time zone)
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// 6. Middleware (ordre important)
app.UseStaticFiles();
app.UseRouting();

// ⚠️ OBLIGATOIRE : Authentification et Autorisation
app.UseAuthentication();  // ← Doit être avant UseAuthorization
app.UseAuthorization();   // ← Doit être après UseAuthentication

// 7. Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();