using Moq;
using UserService.Application.Interfaces;
using UserService.Application.Queries.LoginUser;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;
using UserService.Application.Utils;

namespace UserService.Tests.Handlers;

public class LoginUserHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock = new();
    private readonly LoginUserHandler _handler;

    public LoginUserHandlerTests()
    {
        _handler = new LoginUserHandler(
            _userRepositoryMock.Object,
            _jwtTokenGeneratorMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var password = "password";
        var user = new User
        {
            Name = "TestUser",
            Password = PasswordHasher.HashPassword(password)
        };

        _userRepositoryMock
            .Setup(r => r.GetUserByNameAsync("TestUser"))
            .ReturnsAsync(user);

        _jwtTokenGeneratorMock
            .Setup(j => j.GenerateToken(user))
            .Returns("token");

        var query = new LoginUserQuery("TestUser", password);

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        Assert.Equal("token", result);
    }

    [Fact]
    public async Task HandleAsync_UserNotFound_ThrowsInvalidCredentialsException()
    {
        // Arrange
        _userRepositoryMock
            .Setup(r => r.GetUserByNameAsync("TestUser"))
            .ReturnsAsync((User?)null);

        var query = new LoginUserQuery("TestUser", "password");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(
            () => _handler.HandleAsync(query));
    }

    [Fact]
    public async Task HandleAsync_WrongPassword_ThrowsInvalidCredentialsException()
    {
        // Arrange
        var user = new User
        {
            Name = "TestUser",
            Password = PasswordHasher.HashPassword("correctPassword")
        };

        _userRepositoryMock
            .Setup(r => r.GetUserByNameAsync("TestUser"))
            .ReturnsAsync(user);

        var query = new LoginUserQuery("TestUser", "wrongPassword");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(
            () => _handler.HandleAsync(query));
    }
}
