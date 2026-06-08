using Microsoft.EntityFrameworkCore;
using Shared.Application.Interfaces;
using Shared.Domain.Entities;
using Shared.Infrastructure.Database;

namespace Shared.Infrastructure.Repositories;

public class TokenBlacklistRepository(AppDbContext context) : ITokenBlacklistRepository
{
    public async Task AddAsync(string token, DateTime expiresAt)
    {
        context.BlacklistedTokens.Add(new BlacklistedToken
        {
            Token = token,
            ExpiresAt = expiresAt
        });

        await context.SaveChangesAsync();
    }

    public async Task<bool> IsBlacklistedAsync(string token)
    {
        return await context.BlacklistedTokens
            .AnyAsync(t => t.Token == token && t.ExpiresAt > DateTime.UtcNow);
    }
}
