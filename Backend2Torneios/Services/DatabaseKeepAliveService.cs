using Microsoft.EntityFrameworkCore;

namespace Backend2Torneios.Services;

public class DatabaseKeepAliveService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseKeepAliveService> _logger;

    public DatabaseKeepAliveService(
        IServiceProvider serviceProvider,
        ILogger<DatabaseKeepAliveService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromHours(24));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await context.Database.ExecuteSqlRawAsync(
                    "SELECT 1",
                    stoppingToken);

                _logger.LogInformation("KeepAlive do banco executado em {DataHora}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar KeepAlive do banco.");
            }
        }
    }
}