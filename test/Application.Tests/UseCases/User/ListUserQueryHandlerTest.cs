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
        var users = new List<Domain.Entities.User?>
        {
            Domain.Entities.User.Create("Test User 1", "testUser1", "password1", BaseEntity.GetNewId()),
            Domain.Entities.User.Create("Test User 2", "testUser2", "password2", BaseEntity.GetNewId())
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
            .ReturnsAsync([]);

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
            .ReturnsAsync(null, TimeSpan.FromMilliseconds(1));

        var query = new ListUserQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(OperationResult.NotFound, result.OperationResult);
        Assert.Null(result.Users);
        _userRepositoryMock.Verify(repo => repo.ListUsers(), Times.Once);
    }
}
