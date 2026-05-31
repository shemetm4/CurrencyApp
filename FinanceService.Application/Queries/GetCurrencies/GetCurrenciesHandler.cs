using FinanceService.Domain.Interfaces;
using FinanceService.Domain.Entities;

namespace FinanceService.Application.Queries.GetCurrencies;

public class GetCurrenciesHandler(ICurrencyRepository currencyRepository)
{
    public async Task<IEnumerable<Currency>> HandleAsync(GetCurrenciesQuery query)
    {
        return await currencyRepository.GetCurrenciesByUserIdAsync(query.UserId);
    }
}
