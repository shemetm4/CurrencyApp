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

            var DbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await DbContext.Database.MigrateAsync(stoppingToken);

            logger.LogInformation("Migration completed successfully.");
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Migration failed!");
            throw;
        }
        
    }
}
