using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
