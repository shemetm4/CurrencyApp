using Moq;
using UserService.Application.Commands.RemoveFavorite;
using UserService.Application.Interfaces;

namespace UserService.Tests.Handlers;

public class RemoveFavoriteHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly RemoveFavoriteHandler _handler;

    public RemoveFavoriteHandlerTests()
    {
        _handler = new RemoveFavoriteHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ValidRequest_CallsRemoveFavoriteAsync()
    {
        // Arrange
        var command = new RemoveFavoriteCommand(1, 2);

        // Act
        await _handler.HandleAsync(command);

        // Assert
        _userRepositoryMock.Verify(
            r => r.RemoveFavoriteAsync(1, 2), Times.Once);
    }
}
