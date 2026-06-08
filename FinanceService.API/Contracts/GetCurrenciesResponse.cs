namespace FinanceService.API.Contracts;

public record GetCurrenciesResponse(int Id, string Name, decimal ExchangeRate);
