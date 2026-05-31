namespace FinanceService.Infrastructure.Options;

public class DbConnectionSettings
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string Database { get; init; }

    public string ConnectionString
        => $"Host={Host};Port={Port};Username={Username};Password={Password};Database={Database}";
}
