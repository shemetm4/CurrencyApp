using FinanceService.Application.Interfaces;
using FinanceService.Domain.Entities;

namespace FinanceService.Application.Queries.GetCurrencies;

// todo: interface
public class GetCurrenciesHandler(ICurrencyRepository currencyRepository)
{
    public async Task<IEnumerable<Currency>> HandleAsync(GetCurrenciesQuery query)
    {
        return await currencyRepository.GetCurrenciesByUserIdAsync(query.UserId);
    }
}
