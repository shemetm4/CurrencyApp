namespace FinanceService.Domain.Entities;

public class Currency
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal ExchangeRate { get; set; }
}
