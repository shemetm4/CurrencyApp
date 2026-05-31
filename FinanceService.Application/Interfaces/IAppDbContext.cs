using FinanceService.Domain.Entities;
using Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Currency> Currencies { get; }
    DbSet<UserFavorite> UserFavorites { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
