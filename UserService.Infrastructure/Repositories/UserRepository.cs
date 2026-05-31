using Microsoft.EntityFrameworkCore;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using Shared.Domain.Entities;

namespace UserService.Infrastructure.Repositories;

public class UserRepository(IAppDbContext context) : IUserRepository
{
    public async Task<User?> GetUserByNameAsync(string name)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Name == name);
    }

    public async Task AddUserAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

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
