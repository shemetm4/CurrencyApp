using Shared.Infrastructure.Database;
using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using CurrencyBackgroundService.Options;

namespace CurrencyBackgroundService;

public class Worker(
    IServiceScopeFactory scopeFactory,
    ILogger<Worker> logger,
    IOptions<WorkerSettings> settings) : BackgroundService
{
    private readonly WorkerSettings _settings = settings.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Fetching currencies from CBR...");

            try
            {
                await FetchAndUpdateCurrenciesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching currencies from CBR");
            }

            await Task.Delay(TimeSpan.FromHours(_settings.UpdateIntervalHours), stoppingToken);
        }
    }

    private async Task FetchAndUpdateCurrenciesAsync(CancellationToken stoppingToken)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using var httpClient = new HttpClient();

        var response = await httpClient.GetByteArrayAsync(_settings.CbrUrl, stoppingToken);

        var xml = XDocument.Parse(Encoding.GetEncoding("windows-1251").GetString(response));

        var currencies = xml.Descendants(ValuteXmlElements.Valute)
            .Select(v => new
            {
                Name = v.Element(ValuteXmlElements.CharCode)!.Value,
                Rate = decimal.Parse(
                    v.Element(ValuteXmlElements.VunitRate)!.Value.Replace(",", "."),
                    System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture)
            })
            .ToList();

        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        foreach (var currency in currencies)
        {
            var existing = await dbContext.Currencies
                .FirstOrDefaultAsync(c => c.Name == currency.Name, stoppingToken);

            if (existing is null)
            {
                dbContext.Currencies.Add(new Currency
                {
                    Name = currency.Name,
                    ExchangeRate = currency.Rate
                });
            }
            else
            {
                existing.ExchangeRate = currency.Rate;
            }
        }

        await dbContext.SaveChangesAsync(stoppingToken);
        logger.LogInformation("Currencies updated successfully");
    }
}
