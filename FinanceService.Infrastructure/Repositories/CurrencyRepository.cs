using FinanceService.Application.Interfaces;
using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infrastructure.Repositories;

public class CurrencyRepository(IFinanceDbContext context) : ICurrencyRepository
{
    public async Task<IReadOnlyCollection<Currency>> GetCurrenciesByUserIdAsync(int userId)
    {
        return await context.Currencies
            .Where(c => context.UserFavorites.Any(uf => uf.UserId == userId && uf.CurrencyId == c.Id))
            .ToListAsync();
    }
}
