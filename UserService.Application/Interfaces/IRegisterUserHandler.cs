using UserService.Application.Commands.RegisterUser;

namespace UserService.Application.Interfaces;

public interface IRegisterUserHandler
{
    Task HandleAsync(RegisterUserCommand command);
}
