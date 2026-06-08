using Moq;
using UserService.Application.Commands.AddFavorite;
using UserService.Application.Interfaces;

namespace UserService.Tests.Handlers;

public class AddFavoriteHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly AddFavoriteHandler _handler;

    public AddFavoriteHandlerTests()
    {
        _handler = new AddFavoriteHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ValidRequest_CallsAddFavoriteAsync()
    {
        // Arrange
        var command = new AddFavoriteCommand(1, 2);

        // Act
        await _handler.HandleAsync(command);

        // Assert
        _userRepositoryMock.Verify(
            r => r.AddFavoriteAsync(1, 2), Times.Once);
    }
}
