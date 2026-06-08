using UserService.Application.Queries.LoginUser;

namespace UserService.Application.Interfaces;

public interface ILoginUserHandler
{
    Task<string> HandleAsync(LoginUserQuery query);
}
