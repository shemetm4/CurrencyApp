using UserService.Application.Interfaces;

namespace UserService.Application.Commands.AddFavorite;

// todo: interface
public class AddFavoriteHandler(IUserRepository userRepository)
{
    public async Task HandleAsync(AddFavoriteCommand command)
    {
        await userRepository.AddFavoriteAsync(command.UserId, command.CurrencyId);
    }
}
