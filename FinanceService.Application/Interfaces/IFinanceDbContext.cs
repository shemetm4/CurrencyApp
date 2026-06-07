using FinanceService.Domain.Entities;
using Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Application.Interfaces;

public interface IFinanceDbContext
{
    DbSet<Currency> Currencies { get; }
    DbSet<UserFavorite> UserFavorites { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
