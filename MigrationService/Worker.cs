using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Database;

namespace MigrationService;

public class Worker(IServiceScopeFactory scopeFactory, ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting database migration...");

        try
        {
            using var scope = scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await dbContext.Database.MigrateAsync(stoppingToken);

            logger.LogInformation("Migration completed successfully.");

            Environment.Exit(0);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Migration failed!");
            throw;
        }
    }
}
