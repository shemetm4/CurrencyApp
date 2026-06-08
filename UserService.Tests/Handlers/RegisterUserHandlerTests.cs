using Moq;
using UserService.Application.Commands.RegisterUser;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Tests.Handlers;

public class RegisterUserHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly RegisterUserHandler _handler;

    public RegisterUserHandlerTests()
    {
        _handler = new RegisterUserHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_UserDoesNotExist_RegistersSuccessfully()
    {
        // Arrange
        _userRepositoryMock
            .Setup(r => r.GetUserByNameAsync("TestUser"))
            .ReturnsAsync((User?)null);

        var command = new RegisterUserCommand("TestUser", "password");

        // Act
        await _handler.HandleAsync(command);

        // Assert
        _userRepositoryMock.Verify(r => r.AddUserAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_UserAlreadyExists_ThrowsUserAlreadyExistsException()
    {
        // Arrange
        _userRepositoryMock
            .Setup(r => r.GetUserByNameAsync("TestUser"))
            .ReturnsAsync(new User { Name = "TestUser" });

        var command = new RegisterUserCommand("TestUser", "password");

        // Act & Assert
        await Assert.ThrowsAsync<UserAlreadyExistsException>(() => _handler.HandleAsync(command));
    }
}
