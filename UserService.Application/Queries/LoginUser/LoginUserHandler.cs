using UserService.Application.Utils;
using UserService.Domain.Interfaces;

namespace UserService.Application.Queries.LoginUser;

public class LoginUserHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
{
    public async Task<string?> HandleAsync(LoginUserQuery query)
    {
        var user = await userRepository.GetUserByNameAsync(query.Name);

        if (user is null)
            return null;

        if (!PasswordHasher.VerifyPassword(user.Password, query.Password))
            return null;

        return jwtTokenGenerator.GenerateToken(user);
    }
}
