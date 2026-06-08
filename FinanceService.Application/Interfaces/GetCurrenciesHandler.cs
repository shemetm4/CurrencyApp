using FinanceService.Domain.Entities;

namespace FinanceService.Application.Queries.GetCurrencies;

public interface IGetCurrenciesHandler
{
    Task<IEnumerable<Currency>> HandleAsync(GetCurrenciesQuery query);
}
