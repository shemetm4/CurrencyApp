using UserService.Application.Interfaces;
using UserService.Application.Utils;
using UserService.Domain.Exceptions;

namespace UserService.Application.Queries.LoginUser;

public class LoginUserHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator) : ILoginUserHandler
{
    public async Task<string> HandleAsync(LoginUserQuery query)
    {
        var user = await userRepository.GetUserByNameAsync(query.Name);

        if (user is null)
            throw new InvalidCredentialsException();

        if (!PasswordHasher.VerifyPassword(user.Password, query.Password))
            throw new InvalidCredentialsException();

        return jwtTokenGenerator.GenerateToken(user);
    }
}
