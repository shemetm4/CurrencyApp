using FinanceService.Domain.Entities;

namespace FinanceService.Application.Interfaces;

public interface ICurrencyRepository
{
    Task<IReadOnlyCollection<Currency>> GetCurrenciesByUserIdAsync(int userId);
}
