using CurrencyBackgroundService;
using CurrencyBackgroundService.Options;
using Shared.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Database;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptions<DbConnectionSettings>()
    .Bind(builder.Configuration.GetRequiredSection(nameof(DbConnectionSettings)));

builder.Services.AddOptions<WorkerSettings>()
    .Bind(builder.Configuration.GetRequiredSection(nameof(WorkerSettings)));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var dbSettings = builder.Configuration
        .GetRequiredSection(nameof(DbConnectionSettings))
        .Get<DbConnectionSettings>()!;
    options.UseNpgsql(dbSettings.ConnectionString);
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
