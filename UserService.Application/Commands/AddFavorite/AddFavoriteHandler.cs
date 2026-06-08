using UserService.Application.Interfaces;

namespace UserService.Application.Commands.AddFavorite;

public class AddFavoriteHandler(IUserRepository userRepository) : IAddFavoriteHandler
{
    public async Task HandleAsync(AddFavoriteCommand command)
    {
        await userRepository.AddFavoriteAsync(command.UserId, command.CurrencyId);
    }
}
