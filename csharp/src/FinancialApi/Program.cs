using FinancialApi.Auth;
using FinancialApi.Data;
using FinancialApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Database ──────────────────────────────────────────────────────────────────
// Set "Database:Provider" to "inmemory" in appsettings or environment to use the
// EF Core in-memory provider (used by tests). Default is SQLite file database.
var dbProvider      = builder.Configuration["Database:Provider"] ?? "sqlite";
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Data Source=financial.db";

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    if (dbProvider == "inmemory")
        opt.UseInMemoryDatabase("FinancialDb");
    else
        opt.UseSqlite(connectionString);
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ── Seed on startup ───────────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ── EXISTING: JWT middleware populates HttpContext.User on every request ───────
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();
app.Run();

// Expose Program for WebApplicationFactory in integration tests
public partial class Program { }
