namespace Shared.Application.Interfaces;

public interface ITokenBlacklistRepository
{
    Task AddAsync(string token, DateTime expiresAt);
    Task<bool> IsBlacklistedAsync(string token);
}
