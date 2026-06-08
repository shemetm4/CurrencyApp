using UserService.Application.Interfaces;
using UserService.Application.Utils;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Application.Commands.RegisterUser;

public class RegisterUserHandler(IUserRepository userRepository) : IRegisterUserHandler
{
    public async Task HandleAsync(RegisterUserCommand command)
    {
        var existingUser = await userRepository.GetUserByNameAsync(command.Name);
        
        if (existingUser is not null)
            throw new UserAlreadyExistsException(command.Name);

        var user = new User
        {
            Name = command.Name,
            Password = PasswordHasher.HashPassword(command.Password)
        };

        await userRepository.AddUserAsync(user);
    }
}
