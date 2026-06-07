using Microsoft.EntityFrameworkCore;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using Shared.Domain.Entities;

namespace UserService.Infrastructure.Repositories;

public class UserRepository(IUserDbContext context) : IUserRepository
{
    // name or id?
    // todo: exception for not existing user
    public async Task<User?> GetUserByNameAsync(string name)
    {
        // todo: AsNoTracking() and for other Get-methods too
        return await context.Users
            .FirstOrDefaultAsync(u => u.Name == name);
    }

    // todo: unique login + exception for existing login
    // validation for password (8+ chars?)
    public async Task AddUserAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    // todo: exception for not-existing currency
    // exception for currency that already in favorites
    public async Task AddFavoriteAsync(int userId, int currencyId)
    {
        var favorite = new UserFavorite
        {
            UserId = userId,
            CurrencyId = currencyId
        };
        await context.UserFavorites.AddAsync(favorite);
        await context.SaveChangesAsync();
    }

    // todo: exception for not-existing favorite currency (not existing in user favs)
    public async Task RemoveFavoriteAsync(int userId, int currencyId)
    {
        var favorite = await context.UserFavorites
            .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.CurrencyId == currencyId);

        if (favorite is not null)
        {
            context.UserFavorites.Remove(favorite);
            await context.SaveChangesAsync();
        }
    }
}
