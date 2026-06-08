using FinanceService.Application.Interfaces;
using FinanceService.Application.Queries.GetCurrencies;
using FinanceService.Domain.Entities;
using Moq;

namespace FinanceService.Tests.Handlers;

public class GetCurrenciesHandlerTests
{
    private readonly Mock<ICurrencyRepository> _currencyRepositoryMock = new();
    private readonly GetCurrenciesHandler _handler;

    public GetCurrenciesHandlerTests()
    {
        _handler = new GetCurrenciesHandler(_currencyRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_UserHasFavorites_ReturnsCurrencies()
    {
        // Arrange
        var currencies = new List<Currency>
        {
            new() { Id = 1, Name = "USD", ExchangeRate = 90.5m },
            new() { Id = 2, Name = "EUR", ExchangeRate = 98.3m }
        };

        _currencyRepositoryMock
            .Setup(r => r.GetCurrenciesByUserIdAsync(1))
            .ReturnsAsync(currencies);

        var query = new GetCurrenciesQuery(1);

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task HandleAsync_UserHasNoFavorites_ReturnsEmptyCollection()
    {
        // Arrange
        _currencyRepositoryMock
            .Setup(r => r.GetCurrenciesByUserIdAsync(1))
            .ReturnsAsync(new List<Currency>());

        var query = new GetCurrenciesQuery(1);

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        Assert.Empty(result);
    }
}
