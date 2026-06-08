using Microsoft.EntityFrameworkCore;
using Shared.Domain.Entities;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Infrastructure.Repositories;

public class UserRepository(IUserDbContext context) : IUserRepository
{
    public async Task<User?> GetUserByNameAsync(string name)
    {
        return await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Name == name);
    }

    public async Task AddUserAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task AddFavoriteAsync(int userId, int currencyId)
    {
        var favoriteExists = await context.UserFavorites
            .AnyAsync(uf => uf.UserId == userId && uf.CurrencyId == currencyId);

        if (favoriteExists)
            throw new CurrencyAlreadyInFavoriteException(currencyId);

        var currencyExists = await context.Currencies.AnyAsync(c => c.Id == currencyId);

        if (!currencyExists)
            throw new CurrencyNotExistsException(currencyId);

        var favorite = new UserFavorite
        {
            UserId = userId,
            CurrencyId = currencyId
        };

        await context.UserFavorites.AddAsync(favorite);
        await context.SaveChangesAsync();
    }

    public async Task RemoveFavoriteAsync(int userId, int currencyId)
    {
        var favorite = await context.UserFavorites
            .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.CurrencyId == currencyId);

        if (favorite is null)
            throw new CurrencyNotExistsException(currencyId);

        context.UserFavorites.Remove(favorite);
        await context.SaveChangesAsync();
    }
}
