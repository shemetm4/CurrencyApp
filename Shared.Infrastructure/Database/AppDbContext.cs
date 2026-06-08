using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities;
using Microsoft.Extensions.Logging;
using UserService.Domain.Entities;
using UserService.Application.Interfaces;
using FinanceService.Application.Interfaces;

namespace Shared.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUserDbContext, IFinanceDbContext
{
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<UserFavorite> UserFavorites { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<BlacklistedToken> BlacklistedTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Debug);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

