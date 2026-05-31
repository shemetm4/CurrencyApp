using FinanceService.Domain.Entities;

namespace FinanceService.Domain.Interfaces;

public interface ICurrencyRepository
{
    Task<IEnumerable<Currency>> GetCurrenciesByUserIdAsync(int userId);
}
