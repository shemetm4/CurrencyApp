using FinanceService.Application.Interfaces;
using FinanceService.Domain.Entities;
using FinanceService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infrastructure.Repositories;

public class CurrencyRepository(IAppDbContext context) : ICurrencyRepository
{
    public async Task<IEnumerable<Currency>> GetCurrenciesByUserIdAsync(int userId)
    {
        return await context.Currencies
            .Where(c => context.UserFavorites
                .Any(uf => uf.UserId == userId && uf.CurrencyId == c.Id))
            .ToListAsync();
    }
}
