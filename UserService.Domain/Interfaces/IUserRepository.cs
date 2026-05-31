using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByNameAsync(string name);
    Task AddUserAsync(User user);
    Task AddFavoriteAsync(int userId, int currencyId);
    Task RemoveFavoriteAsync(int userId, int currencyId);
}
