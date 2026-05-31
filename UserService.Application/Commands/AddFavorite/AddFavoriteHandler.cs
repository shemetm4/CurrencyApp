using UserService.Domain.Interfaces;

namespace UserService.Application.Commands.AddFavorite;

public class AddFavoriteHandler(IUserRepository userRepository)
{
    public async Task HandleAsync(AddFavoriteCommand command)
    {
        await userRepository.AddFavoriteAsync(command.UserId, command.CurrencyId);
    }
}
