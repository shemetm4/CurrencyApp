using UserService.Application.Commands.RemoveFavorite;

namespace UserService.Application.Interfaces;

public interface IRemoveFavoriteHandler
{
    Task HandleAsync(RemoveFavoriteCommand command);
}
