using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using Shared.Domain.Entities;

namespace UserService.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserFavorite> UserFavorites { get; set; }

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
