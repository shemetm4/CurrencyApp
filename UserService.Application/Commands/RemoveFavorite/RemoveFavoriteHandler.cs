using UserService.Domain.Interfaces;

namespace UserService.Application.Commands.RemoveFavorite;

public class RemoveFavoriteHandler(IUserRepository userRepository)
{
    public async Task HandleAsync(RemoveFavoriteCommand command)
    {
        await userRepository.RemoveFavoriteAsync(command.UserId, command.CurrencyId);
    }
}
