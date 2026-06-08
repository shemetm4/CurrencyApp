namespace Shared.Domain.Entities;

public class BlacklistedToken
{
    public int Id { get; set; }
    public required string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}
