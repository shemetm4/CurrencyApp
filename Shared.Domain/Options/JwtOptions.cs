namespace Shared.Domain.Options;

// todo: move options from Shared.Domain to Shared.Infrastructure
public class JwtOptions
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string Secret { get; init; }
}
