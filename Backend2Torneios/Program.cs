using Backend2Torneios.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var dbHost = Environment.GetEnvironmentVariable("DB_URL");
var dbUser = Environment.GetEnvironmentVariable("DB_USERNAME");
var dbPass = Environment.GetEnvironmentVariable("DB_PASSWORD");
var dbName = "defaultdb";

var connectionString = $"Host={dbHost};Database={dbName};Username={dbUser};Password={dbPass};Ssl Mode=Require;Trust Server Certificate=true";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao conectar no banco: {ex.Message}");
    }
}

app.MapControllers();

app.Run();