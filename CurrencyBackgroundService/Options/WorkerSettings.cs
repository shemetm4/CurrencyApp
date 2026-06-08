namespace CurrencyBackgroundService.Options;

public class WorkerSettings
{
    public required string CbrUrl { get; init; }
    public required int UpdateIntervalHours { get; init; }
    public required string UserAgent { get; init; }
}
