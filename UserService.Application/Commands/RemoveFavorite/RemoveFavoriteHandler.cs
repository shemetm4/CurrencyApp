using UserService.Application.Interfaces;

namespace UserService.Application.Commands.RemoveFavorite;

// todo: interface
public class RemoveFavoriteHandler(IUserRepository userRepository)
{
    public async Task HandleAsync(RemoveFavoriteCommand command)
    {
        await userRepository.RemoveFavoriteAsync(command.UserId, command.CurrencyId);
    }
}
