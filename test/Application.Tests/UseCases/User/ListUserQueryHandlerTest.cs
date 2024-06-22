namespace Application.Tests.UseCases.User;

public class ListUserQueryHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly ListUserQueryHandler _handler;

    public ListUserQueryHandlerTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new ListUserQueryHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfUsers_WhenUsersExist()
    {
        // Arrange
        var users = new List<UserDto>
        {
            new UserDto("Test User 1", "testUser1")
            {
                Id = Ulid.NewUlid(),
                CreatedAt = DateTime.UtcNow
            },
            new UserDto("Test User 2", "testUser2")
            {
                Id = Ulid.NewUlid(),
                CreatedAt = DateTime.UtcNow
            }
        };

        _userRepositoryMock
            .Setup(repo => repo.ListUsers())
            .ReturnsAsync(users);

        var query = new ListUserQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Users);
        Assert.Equal(users.Count, result.Users.Count);
        _userRepositoryMock.Verify(repo => repo.ListUsers(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        _userRepositoryMock
            .Setup(repo => repo.ListUsers())
            .ReturnsAsync(new List<UserDto>());

        var query = new ListUserQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.Success, result.OperationResult);
        Assert.NotNull(result.Users);
        Assert.Empty(result.Users);
        _userRepositoryMock.Verify(repo => repo.ListUsers(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenListUsersReturnsNull()
    {
        // Arrange
        _userRepositoryMock
            .Setup(repo => repo.ListUsers())
            .ReturnsAsync((List<UserDto>?)null);

        var query = new ListUserQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Empty(result.Users);
        _userRepositoryMock.Verify(repo => repo.ListUsers(), Times.Once);
    }
}
