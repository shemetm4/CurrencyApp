using Shared.Infrastructure.Database;
using Shared.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using MigrationService;

var builder = Host.CreateApplicationBuilder(args);

var dbSettings = builder.Configuration
    .GetRequiredSection(nameof(DbConnectionSettings))
    .Get<DbConnectionSettings>()!;

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(dbSettings.ConnectionString));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
