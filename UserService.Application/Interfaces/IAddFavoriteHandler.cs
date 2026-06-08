using UserService.Application.Commands.AddFavorite;

namespace UserService.Application.Interfaces;

public interface IAddFavoriteHandler
{
    Task HandleAsync(AddFavoriteCommand command);
}
