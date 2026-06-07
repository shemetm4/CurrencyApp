using UserService.Application.Utils;
using UserService.Domain.Entities;
using UserService.Application.Interfaces;

namespace UserService.Application.Commands.RegisterUser;

// todo: interface
public class RegisterUserHandler(IUserRepository userRepository)
{
    public async Task<bool> HandleAsync(RegisterUserCommand command)
    {
        var existingUser = await userRepository.GetUserByNameAsync(command.Name);

        if (existingUser is not null)
            return false;

        var user = new User
        {
            Name = command.Name,
            Password = PasswordHasher.HashPassword(command.Password)
        };

        await userRepository.AddUserAsync(user);

        return true;
    }
}
