using Backend2Torneios.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend2Torneios.Services;

public class DatabaseKeepAliveService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseKeepAliveService> _logger;
    private readonly TimeSpan _intervalo = TimeSpan.FromHours(1);

    public DatabaseKeepAliveService(IServiceProvider serviceProvider, ILogger<DatabaseKeepAliveService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await context.Database.ExecuteSqlRawAsync("SELECT 1", stoppingToken);

                _logger.LogInformation("Keep-alive: banco respondeu em {Hora}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Keep-alive falhou: {Erro}", ex.Message);
            }

            await Task.Delay(_intervalo, stoppingToken);
        }
    }
}