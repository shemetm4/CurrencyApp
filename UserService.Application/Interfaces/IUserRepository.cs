using UserService.Domain.Entities;

namespace UserService.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByNameAsync(string name);
    Task AddUserAsync(User user);
    Task AddFavoriteAsync(int userId, int currencyId);
    Task RemoveFavoriteAsync(int userId, int currencyId);
}
