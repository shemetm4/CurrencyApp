using UserService.Application.Interfaces;

namespace UserService.Application.Commands.RemoveFavorite;

public class RemoveFavoriteHandler(IUserRepository userRepository) : IRemoveFavoriteHandler
{
    public async Task HandleAsync(RemoveFavoriteCommand command)
    {
        await userRepository.RemoveFavoriteAsync(command.UserId, command.CurrencyId);
    }
}
